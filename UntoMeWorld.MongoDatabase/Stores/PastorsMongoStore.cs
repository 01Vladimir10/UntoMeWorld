using UntoMeWorld.Domain.Model;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class PastorsMongoStore : GenericMongoStore<Pastor, string>
    {
        public PastorsMongoStore(MongoDbService service) : 
            base(service, "pastors", p => p.Id, Filter)
        {
            
        }
        private static bool Filter(Pastor p, string query)
        {
            query = query.ToLower();
            return p != null && p.ToString().ToLower().Contains(query);
        }
    }
}