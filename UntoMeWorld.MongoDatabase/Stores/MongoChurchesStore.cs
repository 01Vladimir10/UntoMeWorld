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
    public class MongoChurchesStore : IChurchesStore
    {
        private const string ChurchesCollection = "churches";
        private readonly IMongoCollection<Church> _churches;

        public MongoChurchesStore(MongoDbService service)
        {
            _churches = service.GetCollection<Church>(ChurchesCollection);
        }

        public async Task<IEnumerable<Church>> All()
        {
            try
            {
                return await _churches.Find(_ => true).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Church>> All(string query)
        {
            query = query.ToLower();
            var churches = await _churches.FindAsync(c => c.Name.ToLower().Contains(query));
            return await churches.ToListAsync();
        }

        public async Task<IEnumerable<Church>> All(Predicate<Church> query)
        {
            var result = await _churches.FindAsync(_ => query(_));
            return await result.ToListAsync();
        }

        public async Task<Church> Add(Church church)
        {
            await _churches.InsertOneAsync(church);
            return church;
        }

        public async Task<Church> Update(Church data)
        {
            await _churches.ReplaceOneAsync(c => c.Id == data.Id, data);
            return data;
        }

        public async Task Delete(Church data)
        {
            await _churches.DeleteOneAsync(c => c.Id == data.Id);
        }

        public async Task<IEnumerable<Church>> Add(List<Church> data)
        {
            await _churches.InsertManyAsync(data);
            return data;
        }

        public async Task<IEnumerable<Church>> Update(List<Church> data)
        {
            var tasks =
                from church in data
                let filter = Builders<Church>.Filter.Eq(c => c.Id, church.Id)
                select new ReplaceOneModel<Church>(filter, church);
            await _churches.BulkWriteAsync(tasks);
            return data;
        }

        public async Task Delete(IEnumerable<Church> data)
        {
            var churches = data.Select(c => c.Id).ToHashSet();
            await _churches.DeleteManyAsync(c => churches.Contains(c.Id));
        }
    }
}