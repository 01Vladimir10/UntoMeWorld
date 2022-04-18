namespace UntoMeWorld.WasmClient.Client.Data.Model;

public class SortField
{
    public string FieldName { get; set; }
    public bool Descendent { get; set; }

    public SortField()
    {
        
    }
    public SortField(string fieldName, bool descendent = false)
    {
        FieldName = fieldName;
        Descendent = descendent;
    }
    public static SortField Desc(string fieldName)
    {
        return new SortField(fieldName, true);
    }
    public static SortField Asc(string fieldName)
    {
        return new SortField(fieldName, false);
    }

    public override bool Equals(object obj)
    {
        return obj switch
        {
            null => false,
            SortField other => other.FieldName == FieldName && other.Descendent == Descendent,
            _ => false
        };
    }

    protected bool Equals(SortField other)
    {
        return FieldName == other.FieldName && Descendent == other.Descendent;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FieldName, Descendent);
    }
}