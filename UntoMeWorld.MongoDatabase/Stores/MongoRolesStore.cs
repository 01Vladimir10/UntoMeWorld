using System.Collections.Generic;
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
    }
}