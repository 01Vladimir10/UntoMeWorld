using System.Collections.Generic;
using System.Linq;

namespace UntoMeWorld.Domain.Common
{
    public class QueryFilter
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public string Operator { get; set; } = QueryOperator.Eq;
        public List<QueryFilter> Children { get; set; } = new();

        public override string ToString()
        {
            return Children.Any() ? $"{Operator}({string.Join(", ", Children.Select(x => x.ToString()))})" : $"{Operator}(${PropertyName}, {Value})";
        }
    }
}