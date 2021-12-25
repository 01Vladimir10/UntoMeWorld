namespace UntoMeWorld.WasmClient.Shared.Model;

public class Query
{
    public string QueryString { get; set; } = "";
    public List<OrderType> OrderBy { get; set; } = new();
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 0;
}