using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Infrastructure.Helpers;
using UntoMeWorld.Infrastructure.ReadModels;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores
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