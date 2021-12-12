using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChurchesStore : GenericMongoStore<Church, string>, IChurchesStore
    {
        private const string ChurchesCollection = "churches";
        public MongoChurchesStore(MongoDbService service) : base(service, ChurchesCollection, KeySelector, Filter)
        {
            
        }
        private static string KeySelector(Church c) => c.Id;
        private static bool Filter(Church c, string query) =>
            c != null && !string.IsNullOrWhiteSpace(query) && c.ToString().ToLower().Contains(query!.Trim().ToLower());
    }
}