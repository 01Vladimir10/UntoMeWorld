using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MongoDB.Driver;
using UntoMeWorld.Domain.Common;

namespace UntoMeWorld.MongoDatabase.Helpers
{
    public static class QueryFilterConverter
    {
        public static FilterDefinition<T> Convert<T>(QueryFilter filter)
        {
            if (filter == null)
                return Builders<T>.Filter.Empty;
            if (filter.IsLeaf())
                return filter.Operator switch
                {
                    QueryOperator.Eq => Builders<T>.Filter.Eq(filter.PropertyName, filter.Value),
                    QueryOperator.Lt => Builders<T>.Filter.Lt(filter.PropertyName, filter.Value),
                    QueryOperator.Gt => Builders<T>.Filter.Gt(filter.PropertyName, filter.Value),
                    QueryOperator.Lte => Builders<T>.Filter.Lte(filter.PropertyName, filter.Value),
                    QueryOperator.Gte => Builders<T>.Filter.Gte(filter.PropertyName, filter.Value),
                    QueryOperator.In => Builders<T>.Filter.In(filter.PropertyName, (filter.Value is JsonElement value ? value : default).EnumerateArray()),
                    QueryOperator.TextSearch => Builders<T>.Filter.Text(filter.Value.ToString()),
                    _ => Builders<T>.Filter.Empty
                };
            var results = filter.Children.Select(Convert<T>).ToList();
            return filter.Operator switch
            {
                QueryOperator.Or => Builders<T>.Filter.Or(results),
                QueryOperator.And => Builders<T>.Filter.And(results),
                _ => Builders<T>.Filter.Empty
            };
        }

        private static bool IsLeaf(this QueryFilter filter)
            => filter.Operator is not QueryOperator.And or QueryOperator.Or &&
               !filter.Children.Any();
    }
}