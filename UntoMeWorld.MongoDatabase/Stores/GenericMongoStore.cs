using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Helpers;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public abstract class GenericMongoStore<TModel> : IStore<TModel> where TModel : IModel, IRecyclableModel
    {
        protected readonly IMongoCollection<TModel> Collection;
        protected GenericMongoStore(MongoDbService service, string collection)
        {
            Collection = service.GetCollection<TModel>(collection);
        }

        public async Task<IEnumerable<TModel>> All()
        {
            try
            {
                return await Collection.Find(_ => true).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<TModel>> All(string query)
        {
            var items = await Collection.FindAsync(Builders<TModel>.Filter.Text(query));
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<TModel>> All(Predicate<TModel> query)
        {
            var result = await Collection.FindAsync(_ => query(_));
            return await result.ToListAsync();
        }
        public async Task<PaginationResult<TModel>> Query(QueryFilter filter, string orderBy = null,
            bool orderDesc = false, int page = 1, int pageSize = 100)
        {
            var (totalPages, result) = await Collection.QueryByPageAndSort(filter, orderBy, orderDesc, page, pageSize);
            return new PaginationResult<TModel>
            {
                Result = result.ToList(),
                TotalPages = totalPages,
                Page = page
            };
        }

        public async Task<TModel> Add(TModel church)
        {
            await Collection.InsertOneAsync(church);
            return church;
        }

        public async Task<TModel> Update(TModel data)
        {
            await Collection.ReplaceOneAsync(Builders<TModel>.Filter.Eq(c => c.Id, data.Id), data);
            return data;
        }

        public async Task Delete(TModel data)
        {
            await Collection.DeleteOneAsync(c => c.Id.Equals(data.Id));
        }

        public Task Delete(params string[] ids)
        {
            return Collection.DeleteManyAsync(Builders<TModel>.Filter.In(_ => _.Id, ids));
        }

        public async Task<IEnumerable<TModel>> Add(List<TModel> data)
        {
            await Collection.InsertManyAsync(data);
            return data;
        }

        public async Task<IEnumerable<TModel>> Update(List<TModel> data)
        {
            var tasks =
                from item in data
                let filter = Builders<TModel>.Filter.Eq(c => c.Id, item.Id)
                select new ReplaceOneModel<TModel>(filter, item);
            await Collection.BulkWriteAsync(tasks);
            return data;
        }

        public async Task Delete(IEnumerable<TModel> data)
        {
            var tasks = from item in data
                let filter = Builders<TModel>.Filter.Eq(c => c.Id, item.Id)
                select new DeleteOneModel<TModel>(filter);
            await Collection.BulkWriteAsync(tasks);
        }

        public Task SoftDelete(params string[] ids)
        {
            var filter = Builders<TModel>.Filter.In(_ => _.Id, ids);
            
            var updateAction = Builders<TModel>.Update
                .Set(_ => _.IsDeleted, true)
                .Set(_ => _.DeletedOn, DateTime.Now);

            return Collection.UpdateManyAsync(filter, updateAction);
        }

        public Task PermanentlyDelete(params string[] ids)
        {
            return Collection.DeleteManyAsync(Builders<TModel>.Filter.In(_ => _.Id, ids));
        }

        public Task Restore(params string[] ids)
        {
            var updateAction = Builders<TModel>.Update
                .Set(_ => _.IsDeleted, false)
                .Set(_ => _.LastUpdatedOn, DateTime.Now);

            var updateActions = from id in ids
                let filter = Builders<TModel>.Filter.Eq(m => m.Id, id)
                select new UpdateOneModel<TModel>(filter, updateAction);
            
            return Collection.BulkWriteAsync(updateActions);
        }

        public async Task<TModel> Get(string id)
        {
            var item = await Collection.AsQueryable().FirstOrDefaultAsync(i => i.Id == id);
            return item;
        }
    }
}