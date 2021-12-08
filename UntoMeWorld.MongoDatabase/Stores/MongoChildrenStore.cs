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
    public class MongoChildrenStore : IChildrenStore
    {
        private const string ChildrenCollection = "children";
        private readonly IMongoCollection<Child> _children;

        public MongoChildrenStore(MongoDbService service)
        {
            _children = service.GetCollection<Child>(ChildrenCollection);
        }

        public async Task<IEnumerable<Child>> All()
        {
            var result = await _children.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Child>> All(string query)
        {
            query = query.Trim().ToLower();
            var result = await _children.FindAsync(_ => _.Name.ToLower().Contains(query));
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Child>> All(Predicate<Child> query)
        {
            var result = await _children.FindAsync(_ => query(_));
            return await result.ToListAsync();
        }

        public async Task<Child> Add(Child data)
        {
            await _children.InsertOneAsync(data);
            return data;
        }

        public async Task<Child> Update(Child data)
        {
            await _children.ReplaceOneAsync(c => c.Id == data.Id, data);
            return data;
        }

        public Task Delete(Child data)
        {
            return _children.DeleteOneAsync(c => c.Id == data.Id);
        }

        public async Task<IEnumerable<Child>> Add(List<Child> data)
        {
            await _children.InsertManyAsync(data);
            return data;
        }

        public async Task<IEnumerable<Child>> Update(List<Child> data)
        {
            var operations = from child in data
                select new ReplaceOneModel<Child>(Builders<Child>.Filter.Eq(c => c.Id, child.Id), child);
            await _children.BulkWriteAsync(operations);
            return data;
        }

        public Task Delete(IEnumerable<Child> data)
        {
            var operations = from child in data
                select new DeleteOneModel<Child>(Builders<Child>.Filter.Eq(c => c.Id, child.Id));
            return _children.BulkWriteAsync(operations);
        }
    }
}