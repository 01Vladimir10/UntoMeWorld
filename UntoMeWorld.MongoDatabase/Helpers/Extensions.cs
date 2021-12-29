using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.MongoDatabase.Helpers
{
    public static class Extensions
    {
        public static async Task<(int totalPages, IReadOnlyList<T> readOnlyList)> 
            QueryByPageAndSort<T>(this IMongoCollection<T> collection, IEnumerable<DatabaseQueryParameter> query, string sortBy, bool sortAsc, int page, int pageSize)
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

            var filter = query.ToFilter<T>();
            
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

        private static FilterDefinition<T> ToFilter<T>(this IEnumerable<DatabaseQueryParameter> query)
        { 
            if (query == null)
                return Builders<T>.Filter.Empty;
            
            var parameters = query.ToList();
            
            if (!parameters.Any())
                return Builders<T>.Filter.Empty;

            var filters = parameters.Select(p => p.ToFilter<T>());
            return Builders<T>.Filter.And(filters);
        }

        private static FilterDefinition<T> ToFilter<T>(this DatabaseQueryParameter parameter)
        {
            return parameter.Operator switch
            {
                DatabaseQueryOperator.Equal => Builders<T>.Filter.Eq(parameter.PropertyName, parameter.Value),
                DatabaseQueryOperator.NotEqual => Builders<T>.Filter.Ne(parameter.PropertyName, parameter.Value),
                DatabaseQueryOperator.SmallerThan => Builders<T>.Filter.Lt(parameter.PropertyName, parameter.Value),
                DatabaseQueryOperator.GreaterThan => Builders<T>.Filter.Gt(parameter.PropertyName, parameter.Value),
                DatabaseQueryOperator.GreaterOrEqualThan => Builders<T>.Filter.Gte(parameter.PropertyName,
                    parameter.Value),
                DatabaseQueryOperator.SmallerOrEqualThan => Builders<T>.Filter.Lte(parameter.PropertyName,
                    parameter.Value),
                DatabaseQueryOperator.TextQuery => Builders<T>.Filter.Text(parameter.Value.ToString()),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}