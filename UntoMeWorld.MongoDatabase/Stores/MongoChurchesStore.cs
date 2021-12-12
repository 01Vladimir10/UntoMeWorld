using UntoMeWorld.Domain.Model;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChurchesStore : GenericMongoStore<Church, string>
    {
        public MongoChurchesStore(MongoDbService service) : base(service, "churches", c => c.Id, (c, query) => c.Name.Contains(query))
        {
            
        }
    }
}