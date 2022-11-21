using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.WasmClient.Client.Data.Model;
using UntoMeWorld.WasmClient.Shared.Model;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class PaginationHelper<T>
{
    public bool IsFirstPage => PageIndex == 0;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    private int TotalPages { get; set; } = 1;
    public int TotalItems { get; private set; }
    public bool HasNextPage => PageIndex < TotalPages;
    public QueryFilter? Filter { get; private set; }
    public string SortBy { get; private set; } = "default";
    public bool SortDesc { get; private set; } = true;
    public string? TextQuery { get; set; }
    
    private Func<QueryRequestDto, Task<PaginationResult<T>>> PaginationDelegate { get; }

    public PaginationHelper(Func<QueryRequestDto, Task<PaginationResult<T>>> paginationDelegate)
    {
        PaginationDelegate = paginationDelegate;
    }
    
    public void UpdateSortByField(SortField sortField)
    {
        SortBy = sortField.FieldName;
        SortDesc = sortField.Descendent;
        PageIndex = 0;
    }
    public void UpdateQueryFilter(QueryFilter? filter)
    {
        Filter = filter;
        PageIndex = 0;
    }

    public void Reset()
    {
        PageIndex = 0;
        TotalPages = 1;
    }

    private QueryRequestDto RequestDto => new()
    {
        Filter = Filter,
        OrderBy = SortBy,
        OrderDesc = SortDesc,
        Page = ++PageIndex,
        PageSize = PageSize,
        TextQuery = TextQuery
    };

    public async Task<List<T>> FetchNextPage()
    {
        var pageResult = await PaginationDelegate(RequestDto);
        TotalPages = pageResult.TotalPages;
        TotalItems = pageResult.TotalItems;
        if (TotalItems == 0)
            PageIndex = 0;
        return pageResult.Result;
    }
}
