using MongoDB.Bson;
using MongoDB.Driver;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.Infrastructure.Helpers
{
    public static class MongoConverterExtensions
    {
        public static async Task<(int totalPages, IReadOnlyList<TResult> readOnlyList)>
            QueryByPageAndSort<T, TResult>(this IMongoCollection<T> collection, QueryFilter? queryFilter, 
                string? textQuery,
                string? sortBy,
                bool sortAsc, int page, int pageSize,
                List<IPipelineStageDefinition>? additionalDataStages = null)
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
                var sortOrder = sortAsc ? Builders<T>.Sort.Ascending(sortBy) : Builders<T>.Sort.Descending(sortBy);
                dataStages.Add(PipelineStageDefinitionBuilder.Sort(sortOrder));
            }
            
            dataStages.Add(PipelineStageDefinitionBuilder.Skip<T>((page - 1) * pageSize));
            dataStages.Add(PipelineStageDefinitionBuilder.Limit<T>(pageSize));
            
            additionalDataStages?.ForEach(e => dataStages.Add(e));

            var dataPipeline =
                PipelineDefinition<T, TResult>.Create(dataStages);
            
            var dataFacet = AggregateFacet.Create("data", dataPipeline);
            var filter = queryFilter == null ? Builders<T>.Filter.Empty : QueryFilterConverter.Convert<T>(queryFilter);
            var aggregation = collection.Aggregate();

            if (!string.IsNullOrEmpty(textQuery))
            {
                aggregation =
                    aggregation.AppendStage(AtlasSearch<T>(collection.CollectionNamespace.CollectionName, textQuery));
            }

            var aggregationResults = await aggregation
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

            return ((int) count, data);
        }

        private static PipelineStageDefinition<T, T> AtlasSearch<T>(string collection, string query, string? path = null)
            => new BsonDocumentPipelineStageDefinition<T, T>(
                new BsonDocument()
                    .Add("$search", 
                        new BsonDocument()
                            .Add("index", $"{collection.ToLower()}_search_index")
                            .Add("text", new BsonDocument()
                                .Add("query", query)
                                .Add("fuzzy", new BsonDocument())
                                .Add("path", string.IsNullOrEmpty(path) ? new BsonDocument("wildcard", "*") : path)))
                
                    );
    }
}