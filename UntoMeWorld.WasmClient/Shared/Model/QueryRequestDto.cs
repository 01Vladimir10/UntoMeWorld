using UntoMeWorld.Domain.Common;

namespace UntoMeWorld.WasmClient.Shared.Model;

public class QueryRequestDto
{
    #nullable enable
    public QueryFilter? Filter { get; set; } = null;
    public string OrderBy { get; set; } = string.Empty;
    public bool OrderDesc { get; set; } = false;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 0;
}