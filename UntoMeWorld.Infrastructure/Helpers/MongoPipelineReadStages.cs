﻿using MongoDB.Bson;
using MongoDB.Driver;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Infrastructure.ReadModels;

namespace UntoMeWorld.Infrastructure.Helpers;

public static class MongoPipelineReadStages
{
    public static readonly List<IPipelineStageDefinition>
        ChildrenPipeLineStages =
            new()
            {
                // Convert pastor's id into ObjectId
                new BsonDocumentPipelineStageDefinition<Child, Child>(
                    new BsonDocument("$addFields",
                        new BsonDocument("ChurchObjectId",
                            new BsonDocument("$convert",
                                new BsonDocument
                                {
                                    { "input", "$ChurchId" },
                                    { "to", "objectId" },
                                    { "onError", "" },
                                    { "onNull", "" }
                                })))),
                // Find related pastors (it returns an array)
                new BsonDocumentPipelineStageDefinition<Child, Child>(
                    new BsonDocument("$lookup",
                        new BsonDocument
                        {
                            { "from", "churches" },
                            { "localField", "ChurchObjectId" },
                            { "foreignField", "_id" },
                            { "as", "ChurchesArray" }
                        })
                ),
                // Fetch the first item of the pastors array and save it to the field Called Pastor
                new BsonDocumentPipelineStageDefinition<Child, Child>(
                    new BsonDocument("$addFields",
                        new BsonDocument("Church",
                            new BsonDocument("$first", "$ChurchesArray")))
                ),
                // Remove the pastors array and the pastor object id
                new BsonDocumentPipelineStageDefinition<Child, MongoChildReadModel>(
                    new BsonDocument("$project",
                        new BsonDocument
                        {
                            { "ChurchesArray", 0 },
                            { "ChurchObjectId", 0 }
                        }))
            };
}