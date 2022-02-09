using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public abstract class GenericMongoSecurityStore<T> : ISecurityStore<T> where T : IModel 
    {
        protected IMongoCollection<T> Collection { get; set; }

        public GenericMongoSecurityStore(MongoDbService service, string collection)
        {
            Collection = service.GetCollection<T>(collection);
        }
        
        public Task<List<T>> All()
            => Collection.AsQueryable().ToListAsync();

        public async Task<T> Add(T item)
        {
            await Collection.InsertOneAsync(item);
            return item;
        }

        public async Task<IEnumerable<T>> Add(IEnumerable<T> items)
        {
            var itemsList = items.ToList();
            await Collection.InsertManyAsync(itemsList);
            return itemsList;
        }

        public Task<T> Get(string id)
            => Collection.AsQueryable().FirstOrDefaultAsync(i => i.Id == id);

        public Task Update(params T[] items)
        {
            var tasks =
                from item in items
                let filter = Builders<T>.Filter.Eq(c => c.Id, item.Id)
                select new ReplaceOneModel<T>(filter, item);
            return Collection.BulkWriteAsync(tasks);
        }

        public Task Delete(params string[] ids)
        {
            var tasks =
                from item in ids
                let filter = Builders<T>.Filter.Eq(c => c.Id, item)
                select new DeleteOneModel<T>(filter);
            return Collection.BulkWriteAsync(tasks);
        }
    }
}