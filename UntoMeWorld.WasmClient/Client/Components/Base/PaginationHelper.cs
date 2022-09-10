using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Client.Data.Model;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class PaginationHelper<T>
{
    public bool IsFirstPage => PageIndex == 0;
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 50;
    private int TotalPages { get; set; } = 1;
    public int TotalItems { get; private set; }
    public string TextSearch { get; set; }
    public bool HasNextPage => PageIndex < TotalPages;
    public QueryFilter Filter { get; private set; }
    public string SortBy { get; private set; }
    public bool SortDesc { get; private set; } = true;
    private Func<QueryFilter, string, string, bool, int, int, Task<PaginationResult<T>>> PaginationDelegate { get; }

    public PaginationHelper(Func<QueryFilter, string, string, bool, int, int, Task<PaginationResult<T>>> paginationDelegate)
    {
        PaginationDelegate = paginationDelegate;
    }
    
    public void UpdateSortByField(SortField sortField)
    {
        SortBy = sortField.FieldName;
        SortDesc = sortField.Descendent;
        PageIndex = 0;
    }
    public void UpdateQueryFilter(QueryFilter filter)
    {
        Filter = filter;
        PageIndex = 0;
    }

    public void Reset()
    {
        PageIndex = 0;
        TotalPages = 1;
    }

    public async Task<List<T>> FetchNextPage()
    {
        var pageResult = await PaginationDelegate(Filter, TextSearch, SortBy, SortDesc, ++PageIndex, PageSize);
        TotalPages = pageResult.TotalPages;
        TotalItems = pageResult.TotalItems;
        if (TotalItems == 0)
            PageIndex = 0;
        return pageResult.Result;
    }
}