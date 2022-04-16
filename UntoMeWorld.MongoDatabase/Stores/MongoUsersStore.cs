using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoUsersStore : GenericMongoStore<AppUser, AppUser>, IUserStore
    {
        private const string UsersCollectionName = "users";
        public MongoUsersStore(MongoDbService service) : base(service, UsersCollectionName)
        {
            
        }
        private Task ChangeUserStatus(bool isDisabled, params string[] usersIds)
        {
            var updateAction = Builders<AppUser>.Update.Set(u => u.IsDisabled, isDisabled);
            var tasks = from userId in usersIds
                let filter = Builders<AppUser>.Filter.Eq(t => t.Id, userId)
                select new UpdateOneModel<AppUser>(filter, updateAction);
            return Collection.BulkWriteAsync(tasks);
        }

        public Task Disable(params string[] userIds)
            => ChangeUserStatus(true, userIds);

        public Task Enable(params string[] userIds)
            => ChangeUserStatus(false, userIds);

        public Task<AppUser> GetByThirdPartyUserId(string provider, string providerUserId)
            => Collection
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.AuthProvider == provider && user.AuthProviderUserId == providerUserId);
    }
}