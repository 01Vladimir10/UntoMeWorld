using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChildrenStore : GenericMongoStore<Child, string>, IChildrenStore
    {
        private const string ChildrenCollectionName = "children";

        public MongoChildrenStore(MongoDbService service) :
            base(service, ChildrenCollectionName, KeySelector, Filter)
        {
            // Magic, there is nothing else to be done!!! 
        }

        private static string KeySelector(Child c) => c.Id;


        private static bool Filter(Child child, string query)
            => child != null && !string.IsNullOrWhiteSpace(query) &&
               child.ToString().ToLower().Contains(query.Trim().ToLower());
    }
}