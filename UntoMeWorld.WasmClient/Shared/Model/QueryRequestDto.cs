using UntoMeWorld.Application.Common;
using UntoMeWorld.WasmClient.Shared.Errors;

namespace UntoMeWorld.WasmClient.Shared.Model;

public class QueryRequestDto
{
#nullable enable
    public QueryFilter? Filter { get; set; }
    public string? OrderBy { get; set; } = string.Empty;
    public bool OrderDesc { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? TextQuery { get; set; }

    public void Validate<T>()
    {
        if (Page < 1 || PageSize < 1)
            throw new InvalidPageNumberException();
        if (
            !string.IsNullOrEmpty(OrderBy) &&
            !OrderBy.Equals("default", StringComparison.InvariantCultureIgnoreCase) &&
            !typeof(T).GetProperties().Any(p => p.Name.Equals(OrderBy, StringComparison.InvariantCultureIgnoreCase)))
            throw new InvalidSortByProperty();
    }
}