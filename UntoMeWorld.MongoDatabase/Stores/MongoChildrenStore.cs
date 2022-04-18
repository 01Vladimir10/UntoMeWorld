using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Helpers;
using UntoMeWorld.MongoDatabase.ReadModels;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChildrenStore : GenericMongoStore<Child, MongoChildReadModel>, IChildrenStore
    {
        private const string ChildrenCollectionName = "children";
        public MongoChildrenStore(MongoDbService service) :
            base(service, ChildrenCollectionName,
                readModel =>
                {
                    var child = readModel as Child;
                    child.Church = readModel.Church;
                    return child;
                },
                MongoPipelineReadStages.ChildrenPipeLineStages)
        {
            // Magic, there is nothing else to be done!!! 
        }
    }
}