using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Common;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoRolesStore : GenericMongoSecurityStore<Role>, IRoleStore
    {
        private readonly IMongoCollection<AppUser> _usersCollection;
        public MongoRolesStore(MongoDbService service) : base(service, DbCollections.Roles)
        {
            _usersCollection = service.GetCollection<AppUser>(DbCollections.Users);
        }

        public async Task<List<Role>> GetByUser(string userId)
        {
            var user = await _usersCollection.AsQueryable().FirstOrDefaultAsync();
            if (user == null || !(user.Roles?.Any() ?? false))
            {
                return new List<Role>();
            }
            var roles = user.Roles;
            var result = await Collection.FindAsync(Builders<Role>.Filter.In(r => r.Name, roles));
            return await result.ToListAsync();
        }

        public Task<Role> GetByRoleName(string roleName)
            => Collection.AsQueryable().FirstOrDefaultAsync(r => r.Name == roleName);

        public async Task<List<Role>> GetByRoleName(IEnumerable<string> roleNames)
        {
            var query = Builders<Role>.Filter.In(r => r.Name, roleNames.Distinct());
            var result = await Collection.FindAsync(query);
            var roles = await result.ToListAsync();
            return roles;
        }
    }
}