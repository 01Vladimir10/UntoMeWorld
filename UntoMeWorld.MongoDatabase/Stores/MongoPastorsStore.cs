using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoPastorsStore : GenericMongoStore<Pastor>, IPastorsStore
    {
        public MongoPastorsStore(MongoDbService service) : 
            base(service, "pastors")
        {
            
        }
    }
}