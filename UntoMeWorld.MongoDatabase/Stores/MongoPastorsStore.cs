using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoPastorsStore : GenericMongoStore<Pastor, string>, IPastorsStore
    {
        public MongoPastorsStore(MongoDbService service) : 
            base(service, "pastors", p => p.Id, Filter)
        {
            
        }
        private static bool Filter(Pastor p, string query)
         => p != null && !string.IsNullOrWhiteSpace(query) && p.ToString().ToLower().Contains(query.ToLower());
    }
}