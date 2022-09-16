using UntoMeWorld.Application.Common;

namespace UntoMeWorld.Domain.Common
{
    public static class QueryLanguage
    {
        public static QueryFilter And(QueryFilter item1, QueryFilter item2, params QueryFilter[] items)
            => new()
            {
                Operator = QueryOperator.And,
                Children = new List<QueryFilter>(
                    new[] { item1, item2 }.Concat(items ?? ArraySegment<QueryFilter>.Empty))
            };

        public static QueryFilter Or(QueryFilter item1, QueryFilter item2, params QueryFilter[] items)
            => new()
            {
                Operator = QueryOperator.Or,
                Children = new List<QueryFilter>(
                    new[] { item1, item2 }.Concat(items ?? ArraySegment<QueryFilter>.Empty))
            };

        public static QueryFilter Eq(string property, IComparable value)
            => ComparableFilter(property, value, QueryOperator.Eq);
        public static QueryFilter Ne(string property, IComparable value)
            => ComparableFilter(property, value, QueryOperator.Ne);
        public static QueryFilter Lt(string property, IComparable value)
            => ComparableFilter(property, value, QueryOperator.Lt);

        public static QueryFilter Lte(string property, IComparable value)
            => ComparableFilter(property, value, QueryOperator.Lte);

        public static QueryFilter Gt(string property, IComparable value)
            => ComparableFilter(property, value, QueryOperator.Gt);

        public static QueryFilter Gte(string property, IComparable value)
            => ComparableFilter(property, value, QueryOperator.Gte);

        public static QueryFilter TextSearch(string query)
            => new()
            {
                Operator = QueryOperator.TextSearch,
                Value = query
            };

        public static QueryFilter In<T>(string property, IEnumerable<T> items)
            => new()
            {
                Operator = QueryOperator.In,
                PropertyName = property,
                Value = items
            };

        private static QueryFilter ComparableFilter(string property, IComparable value, string op) 
            => new()
        {
            PropertyName = property,
            Value = value,
            Operator = op
        };
    }
}