using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores
{
    public class MongoTokensStore : GenericMongoSecurityStore<Token>, ITokenStore
    {
        private const string CollectionName = "tokens";
        
        public MongoTokensStore(MongoDbService service) : base(service, CollectionName)
        {
        }

        public Task<List<Token>> GetByUser(string userId)
            => Collection.AsQueryable().Where(t => t.UserId == userId).ToListAsync();


        public Task EnableToken(params string[] tokens)
        {
            var updateAction = Builders<Token>.Update.Set(t => t.IsDisabled, false);
            var tasks = from token in tokens
                let filter = Builders<Token>.Filter.Eq(t => t.Id, token)
                select new UpdateOneModel<Token>(filter, updateAction);
            return Collection.BulkWriteAsync(tasks);
        }

        public Task DisableToken(params string[] tokens)
        {
            var updateAction = Builders<Token>.Update.Set(t => t.IsDisabled, true);
            var tasks = from token in tokens
                let filter = Builders<Token>.Filter.Eq(t => t.Id, token)
                select new UpdateOneModel<Token>(filter, updateAction);
            return Collection.BulkWriteAsync(tasks);
        }
    }
}