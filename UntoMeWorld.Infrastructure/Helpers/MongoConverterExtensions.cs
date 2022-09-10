using MongoDB.Bson;
using MongoDB.Driver;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Infrastructure.Helpers
{
    public static class MongoConverterExtensions
    {
        public static async Task<(int totalPages, IReadOnlyList<TResult> readOnlyList)>
            QueryByPageAndSort<T, TResult>(this IMongoCollection<T> collection, QueryFilter queryFilter, string sortBy,
                bool sortAsc, int page, int pageSize,
                string query = null,
                List<IPipelineStageDefinition> additionalDataStages = null)
            where T : IModel

        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<T, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<T>()
                }));



            var dataStages = new List<IPipelineStageDefinition>();
            
            if (!string.IsNullOrEmpty(sortBy) && !sortBy.Equals("default", StringComparison.InvariantCultureIgnoreCase))
            {
                var sortOrder = string.IsNullOrEmpty(sortBy) ? Builders<T>.Sort.Descending(_ => _.Id) :
                    sortAsc ? Builders<T>.Sort.Ascending(sortBy) : Builders<T>.Sort.Descending(sortBy);
                dataStages.Add(PipelineStageDefinitionBuilder.Sort(sortOrder));
            }
            
            dataStages.AddRange(new []
            {
                PipelineStageDefinitionBuilder.Skip<T>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<T>(pageSize)
            });

            additionalDataStages?.ForEach(e => dataStages.Add(e));

            var dataPipeline =
                PipelineDefinition<T, TResult>.Create(dataStages);


            var dataFacet = AggregateFacet.Create("data", dataPipeline);


            var filter = queryFilter == null ? Builders<T>.Filter.Empty : QueryFilterConverter.Convert<T>(queryFilter);


            var mongoQuery = collection.Aggregate();
            if (!string.IsNullOrEmpty(query))
            {
                mongoQuery = mongoQuery
                        .AppendStage(TextSearchFilter<T>(collection.CollectionNamespace.CollectionName, query));
            }

            var aggregationResults = await mongoQuery
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            if (!aggregationResults?.Any() ?? true)
                return (0, new List<TResult>());

            var first = aggregationResults[0]
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                .FirstOrDefault();

            var count = first?.Count ?? 0;

            var data = aggregationResults.First()
                .Facets.First(x => x.Name == "data")
                .Output<TResult>();

            return ((int)count, data);
        }

        private static BsonDocumentPipelineStageDefinition<T, T> TextSearchFilter<T>(string collection, string query,
            string path = null)
            => new(
                new BsonDocument("$search",
                    new BsonDocument()
                        .Add("index", $"{collection.ToLower()}_search_index")
                        .Add("text", new BsonDocument()
                            .Add("query", query)
                            .Add("path", string.IsNullOrEmpty(path) ? new BsonDocument("wildcard", "*") : path)
                            .Add("fuzzy", new BsonDocument())
                        ))
            );
    }
}