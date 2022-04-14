using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Helpers;
using UntoMeWorld.MongoDatabase.Services;

namespace UntoMeWorld.MongoDatabase.Stores
{
    public class MongoChurchesStore : GenericMongoStore<Church>, IChurchesStore
    {
        private const string ChurchesCollection = "churches";

        public MongoChurchesStore(MongoDbService service) : base(service, ChurchesCollection)
        {
        }

        public new async Task<PaginationResult<Church>> Query(QueryFilter filter = null, string orderBy = null,
            bool orderByDesc = false, int page = 1,
            int pageSize = 100)
        {
            var (totalPages, churches) = await Collection
                .QueryByPageAndSort<Church, MongoChurchReadModel>(filter, orderBy, orderByDesc, page,
                    pageSize,
                    new List<IPipelineStageDefinition>
                    {
                        new BsonDocumentPipelineStageDefinition<Church, Church>(
                            new BsonDocument("$addFields",
                                new BsonDocument("PastorObjectId",
                                    new BsonDocument("$convert",
                                        new BsonDocument
                                        {
                                            { "input", "$PastorId" },
                                            { "to", "objectId" },
                                            { "onError", "" },
                                            { "onNull", "" }
                                        })))),
                        new BsonDocumentPipelineStageDefinition<Church, Church>(
                            new BsonDocument("$lookup",
                                new BsonDocument
                                {
                                    { "from", "pastors" },
                                    { "localField", "PastorObjectId" },
                                    { "foreignField", "_id" },
                                    { "as", "PastorArray" }
                                })
                        ),
                        new BsonDocumentPipelineStageDefinition<Church, Church>(
                            new BsonDocument("$addFields",
                                new BsonDocument("Pastor",
                                    new BsonDocument("$first", "$PastorArray")))
                        ),
                        new BsonDocumentPipelineStageDefinition<Church, MongoChurchReadModel>(
                            new BsonDocument("$project",
                                new BsonDocument
                                {
                                    { "PastorArray", 0 },
                                    { "PastorObjectId", 0 }
                                }))
                    });
            return new PaginationResult<Church>
            {
                TotalPages = totalPages,
                Result = churches.Select(c =>
                {
                    var church = c as Church;
                    church.Pastor = c.Pastor;
                    return church;
                }).ToList()
            };
        }
    }
}