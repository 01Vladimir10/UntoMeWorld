using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoRolesStore : GenericMongoSecurityStore<Role>, IRoleStore
    {
        private const string RolesCollectionName = "roles";
        public MongoRolesStore(MongoDbService service) : base(service, RolesCollectionName)
        {
            
        }
        public Task<List<Role>> GetByUser(string userId)
            => Collection.AsQueryable().Where(r => r.Id == userId).ToListAsync();

        public Task<Role> GetByRoleName(string roleName)
            => Collection.AsQueryable().FirstOrDefaultAsync(r => r.Name == roleName);

        public async Task<IDictionary<string, Role>> GetByRoleName(params string[] roleNames)
        {
            var query = Builders<Role>.Filter.In(r => r.Name, roleNames.Distinct());
            var result = await Collection.FindAsync(query);
            var roles = await result.ToListAsync();
            return roles.ToDictionary(r => r.Name, r => r);
        }
    }
}