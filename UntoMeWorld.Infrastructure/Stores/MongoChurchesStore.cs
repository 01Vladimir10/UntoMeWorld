using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores
{
    public class MongoChurchesStore : GenericMongoStore<Church>, IChurchesStore
    {
        private const string ChurchesCollection = "churches";

        public MongoChurchesStore(MongoDbService service) :
            base(service, ChurchesCollection)
        {
            
        }
    }
}