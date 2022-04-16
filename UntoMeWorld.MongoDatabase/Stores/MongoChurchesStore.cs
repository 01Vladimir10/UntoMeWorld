using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Helpers;
using UntoMeWorld.MongoDatabase.ReadModels;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChurchesStore : GenericMongoStore<Church, MongoChurchReadModel>, IChurchesStore
    {
        private const string ChurchesCollection = "churches";

        public MongoChurchesStore(MongoDbService service) :
            base(service, ChurchesCollection, c =>
            {
                var church = c as Church;
                church.Pastor = c.Pastor;
                return church;
            }, MongoPipelineReadStages.ChurchPipeLineStages)
        {
            
        }
    }
}