using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;

namespace UntoMeWorld.MongoDatabase.Helpers
{
    public static class Extensions
    {
        public static async Task<(int totalPages, IReadOnlyList<T> readOnlyList)> 
            QueryByPageAndSort<T>(this IMongoCollection<T> collection, QueryFilter queryFilter, string sortBy, bool sortAsc, int page, int pageSize)
            where T : IModel
        
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<T, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<T>()
                }));

            var sortOrder = string.IsNullOrEmpty(sortBy) ? Builders<T>.Sort.Descending(_ => _.Id) :
                sortAsc ? Builders<T>.Sort.Ascending(sortBy) : Builders<T>.Sort.Descending(sortBy);

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<T, T>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortOrder),
                    PipelineStageDefinitionBuilder.Skip<T>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<T>(pageSize),
                }));

            var filter = queryFilter == null ? Builders<T>.Filter.Empty : QueryFilterConverter.Convert<T>(queryFilter);

            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();
            
            if (!aggregation?.Any() ?? true)
                return (0 , new List<T>());

            var first = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>().FirstOrDefault();

            var count = first
                ?.Count ?? 0;

            var totalPages = (int)count / pageSize;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<T>();

            return (totalPages, data);
        }
    }
}