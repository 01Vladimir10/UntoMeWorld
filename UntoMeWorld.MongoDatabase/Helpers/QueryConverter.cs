using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using MongoDB.Driver;
using Newtonsoft.Json;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Errors;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UntoMeWorld.MongoDatabase.Helpers
{
    public static class QueryFilterConverter
    {
        public static FilterDefinition<T> Convert<T>(QueryFilter filter)
        {
            try
            {

                if (filter == null)
                    return Builders<T>.Filter.Empty;
                if (filter.IsLeaf())
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
            catch (Exception)
            {
                throw new InvalidQueryFilterException();
            }
        }

        private static object Deserialize(object element)
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

        private static IEnumerable<object> DeserializeArray(object param)
        {
            if (param is JsonElement jsonElement)
                return jsonElement.Deserialize<IEnumerable<object>>()?.Select(Deserialize);
            return param as IEnumerable<object>;
        }

        private static bool IsLeaf(this QueryFilter filter)
            => filter.Operator is not QueryOperator.And or QueryOperator.Or &&
               !filter.Children.Any();
    }
}