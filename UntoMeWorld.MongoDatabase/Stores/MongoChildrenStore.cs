using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChildrenStore : GenericMongoStore<Child>, IChildrenStore
    {
        private const string ChildrenCollectionName = "children";

        public MongoChildrenStore(MongoDbService service) :
            base(service, ChildrenCollectionName)
        {
            // Magic, there is nothing else to be done!!! 
        }
    }
}