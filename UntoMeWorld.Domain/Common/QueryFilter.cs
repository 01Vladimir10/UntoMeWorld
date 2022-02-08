using System.Collections.Generic;

namespace UntoMeWorld.Domain.Common
{
    public class QueryFilter
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public QueryOperator Operator { get; set; }
        public List<QueryFilter> Children { get; set; } = new();
    }
}