using System.Text.Json;
using MongoDB.Driver;
using UntoMeWorld.Domain.Errors;
using UntoMeWorld.Domain.Query;

namespace UntoMeWorld.Infrastructure.Helpers
{
    public static class QueryFilterConverter
    {
        public static FilterDefinition<T> Convert<T>(QueryFilter? filter, HashSet<string>? propertyNames = null)
        {
            try
            {
                if (filter == null)
                    return Builders<T>.Filter.Empty;
                
                propertyNames ??= GetPropertyNames<T>();
                
                if (filter.IsLeaf())
                {
                    if (!filter.Operator.Equals(QueryOperator.TextSearch) && !propertyNames.Contains(filter.PropertyName))
                        throw new Exception(
                            $"{typeof(T).Name} does not contain a property with the name of ({filter.PropertyName})");

                    return filter.Operator switch
                    {
                        QueryOperator.Eq => Builders<T>.Filter.Eq(filter.PropertyName, Deserialize(filter.Value)),
                        QueryOperator.Ne => Builders<T>.Filter.Ne(filter.PropertyName, Deserialize(filter.Value)),
                        QueryOperator.Lt => Builders<T>.Filter.Lt(filter.PropertyName, Deserialize(filter.Value)),
                        QueryOperator.Gt => Builders<T>.Filter.Gt(filter.PropertyName, Deserialize(filter.Value)),
                        QueryOperator.Lte => Builders<T>.Filter.Lte(filter.PropertyName, Deserialize(filter.Value)),
                        QueryOperator.Gte => Builders<T>.Filter.Gte(filter.PropertyName, Deserialize(filter.Value)),
                        QueryOperator.In => Builders<T>.Filter.In(filter.PropertyName, DeserializeArray(filter.Value)),
                        QueryOperator.TextSearch => Builders<T>.Filter.Text(filter.Value.ToString()),
                        _ => throw new Exception($"Unknown operator {filter.Operator}")
                    };
                }
                
                var results = filter.Children.Select(p => Convert<T>(p, propertyNames)).ToList();
                
                if (results.Count < 2)
                    throw new Exception("And & Or operators must have at least 2 children");
                
                return filter.Operator switch
                {
                    QueryOperator.Or => Builders<T>.Filter.Or(results),
                    QueryOperator.And => Builders<T>.Filter.And(results),
                    _ => Builders<T>.Filter.Empty
                };
            }
            catch (Exception e)
            {
                throw new InvalidQueryFilterException(e.Message);
            }
        }

        private static HashSet<string> GetPropertyNames<T>()
        => new HashSet<string>(typeof(T).GetProperties().Select(p => p.Name));

        private static object? Deserialize(object element)
        {
            if (element is JsonElement jsonElement)
                return jsonElement.ValueKind switch
                {
                    JsonValueKind.Undefined => null,
                    JsonValueKind.String => jsonElement.Deserialize<string>(),
                    JsonValueKind.Number => jsonElement.Deserialize<decimal>(),
                    JsonValueKind.Array => DeserializeArray(element),
                    JsonValueKind.True => jsonElement.Deserialize<bool>(),
                    JsonValueKind.False => jsonElement.Deserialize<bool>(),
                    JsonValueKind.Null => null,
                    _ => new object()
                };
            return element;
        }

        private static IEnumerable<object?>? DeserializeArray(object param)
        {
            if (param is JsonElement jsonElement)
                return jsonElement.Deserialize<IEnumerable<object>>()?.Select(Deserialize);
            return param as IEnumerable<object>;
        }

        private static bool IsLeaf(this QueryFilter filter)
            => !filter.Operator.Equals(QueryOperator.And, StringComparison.InvariantCultureIgnoreCase) &&
               !filter.Operator.Equals(QueryOperator.Or, StringComparison.InvariantCultureIgnoreCase);
    }
}