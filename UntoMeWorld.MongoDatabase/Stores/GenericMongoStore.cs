using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public abstract class GenericMongoStore<TModel, TKey> : IStore<TModel>
    {
        private readonly Func<TModel, TKey> _keySelector;
        private readonly Func<TModel, string, bool> _queryFunction;
        private readonly IMongoCollection<TModel> _collection;

        protected GenericMongoStore(MongoDbService service, string collection, Func<TModel, TKey> keySelector, Func<TModel, string, bool> queryFunction)
        {
            _collection = service.GetCollection<TModel>(collection);
            _keySelector = keySelector;
            _queryFunction = queryFunction;
        }

        public async Task<IEnumerable<TModel>> All()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<TModel>> All(string query)
        {
            query = query.ToLower();
            var churches = await _collection.FindAsync(c => _queryFunction(c, query));
            return await churches.ToListAsync();
        }

        public async Task<IEnumerable<TModel>> All(Predicate<TModel> query)
        {
            var result = await _collection.FindAsync(_ => query(_));
            return await result.ToListAsync();
        }

        public async Task<TModel> Add(TModel church)
        {
            await _collection.InsertOneAsync(church);
            return church;
        }

        public async Task<TModel> Update(TModel data)
        {
            await _collection.ReplaceOneAsync(c => _keySelector(c).Equals(_keySelector(data)), data);
            return data;
        }

        public async Task Delete(TModel data)
        {
            await _collection.DeleteOneAsync(c => _keySelector(c).Equals(_keySelector(data)));
        }

        public async Task<IEnumerable<TModel>> Add(List<TModel> data)
        {
            await _collection.InsertManyAsync(data);
            return data;
        }

        public async Task<IEnumerable<TModel>> Update(List<TModel> data)
        {
            var tasks =
                from item in data
                let filter = Builders<TModel>.Filter.Eq(c => _keySelector(c), _keySelector(item))
                select new ReplaceOneModel<TModel>(filter, item);
            await _collection.BulkWriteAsync(tasks);
            return data;
        }

        public async Task Delete(IEnumerable<TModel> data)
        {
            var churches = data.Select(c => _keySelector(c)).ToHashSet();
            await _collection.DeleteManyAsync(c => churches.Contains(_keySelector(c)));
        }
    }
}