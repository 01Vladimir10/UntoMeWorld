using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChurchesRepository : IChurchesRepository
    {
        private const string ChurchesCollection = "churches";
        private readonly IMongoCollection<IChurch> _churches;

        public MongoChurchesRepository(MongoDbService service)
        {
            _churches = service.GetCollection<IChurch>(ChurchesCollection);
        }

        public async Task<IEnumerable<IChurch>> GetAll()
        {
            return await _churches.AsQueryable().ToListAsync();
        }

        public async Task<IEnumerable<IChurch>> GetByQuery(string query)
        {
            query = query.ToLower();
            var churches = await _churches.FindAsync(c => c.Name.ToLower().Contains(query));
            return await churches.ToListAsync();
        }

        public async Task<IEnumerable<IChurch>> GetByQuery(Predicate<IChurch> query)
        {
            var result = await _churches.FindAsync(_ => query(_));
            return await result.ToListAsync();
        }

        public async Task<IChurch> Insert(IChurch data)
        {
            await _churches.InsertOneAsync(data);
            return data;
        }

        public async Task<IChurch> Modify(IChurch data)
        {
            await _churches.ReplaceOneAsync(c => c.Id == data.Id, data);
            return data;
        }

        public async Task Remove(IChurch data)
        {
            await _churches.DeleteOneAsync(c => c.Id == data.Id);
        }

        public async Task<IEnumerable<IChurch>> Insert(IEnumerable<IChurch> data)
        {
            var enumerable = data as IChurch[] ?? data.ToArray();
            await _churches.InsertManyAsync(enumerable);
            return enumerable;
        }

        public Task<IEnumerable<IChurch>> Modify(IEnumerable<IChurch> data)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(IEnumerable<IChurch> data)
        {
            var churches = data.Select(c => c.Id).ToHashSet();
            await _churches.DeleteManyAsync(c => churches.Contains(c.Id));
        }
    }
}