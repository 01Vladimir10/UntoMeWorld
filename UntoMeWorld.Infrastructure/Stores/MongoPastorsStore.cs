using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores
{
    public class MongoPastorsStore : GenericMongoStore<Pastor, Pastor>, IPastorsStore
    {
        public MongoPastorsStore(MongoDbService service) : 
            base(service, "pastors")
        {
            
        }
    }
}