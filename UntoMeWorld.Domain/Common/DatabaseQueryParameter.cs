namespace UntoMeWorld.Domain.Common
{
    public enum DatabaseQueryOperator
    {
        Equal,
        NotEqual,
        SmallerThan,
        GreaterThan,
        GreaterOrEqualThan,
        SmallerOrEqualThan,
        TextQuery
    }
    
    public class DatabaseQueryParameter
    {
        public string PropertyName { get; set; }
        public DatabaseQueryOperator Operator { get; set; }
        public object Value { get; set; }
    }
}