using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChurchesStore : GenericMongoStore<Church>, IChurchesStore
    {
        private const string ChurchesCollection = "churches";
        public MongoChurchesStore(MongoDbService service) : base(service, ChurchesCollection)
        {
            
        }
    }
}