using Microsoft.AspNetCore.Components.Web.Virtualization;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Domain.Query;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Data.Model;

namespace UntoMeWorld.WasmClient.Client.Components.ListControls;

public class ListController<TKey, TItem>
{
    private readonly HashSet<TKey> _selectedItems = new();
    private readonly Func<TItem, TKey> _keySelector;
    private readonly PaginationHelper<TItem> _paginationHelper;
    private string _query = string.Empty;
    public Func<Task> OnDataRefresh { get; set; } = () => Task.CompletedTask;
    public List<ListItem<TKey, TItem>> Items { get; }
    public ItemsProviderDelegate<ListItem<TKey, TItem>> ItemsProvider { get; }
    public int SelectedItemsCount { get; set; }
    public bool IsMultiSelecting { get; set; }

    private readonly Action _onReRender;
    private SortField _sortField;

    public ListController(Func<TItem, TKey> keySelector, PaginationHelper<TItem> paginationHelper, Action onReRender)
    {
        _keySelector = keySelector;
        _paginationHelper = paginationHelper;
        _onReRender = onReRender;
        ItemsProvider = GetItems;
        Items = new List<ListItem<TKey, TItem>>();
    }
    

    private async ValueTask<ItemsProviderResult<ListItem<TKey, TItem>>> GetItems(ItemsProviderRequest request)
    {
        if (request.StartIndex + request.Count > Items.Count)
            await CallApi();
        
        return new ItemsProviderResult<ListItem<TKey, TItem>>(
            Items.Skip(request.StartIndex).Take(request.Count).ToList(), _paginationHelper.TotalItems);
    }

    private async Task CallApi()
    {
        if (!_paginationHelper.HasNextPage)
            return;
        if (_paginationHelper.IsFirstPage)
            Items.Clear();
        var items = await _paginationHelper.FetchNextPage();
        Items.AddRange(items.Select(item => new ListItem<TKey, TItem>
        {
            Key = _keySelector(item),
            Item = item,
            IsSelected = false,
            OnSelectionChanged = OnItemSelectionChanged
        }));
    }
    public Task SetFilter(QueryFilter? filter)
    {
        _paginationHelper.UpdateQueryFilter(filter);
        return RefreshAsync();
    }

    public Task SetTextQuery(string query)
    {
        _paginationHelper.TextQuery = query;
        return RefreshAsync();
    }
    public Task SetSortField(SortField sort)
    {
        _paginationHelper.UpdateSortByField(sort);
        return RefreshAsync();
    }
    private void OnItemSelectionChanged(ListItem<TKey, TItem> item, bool isSelected)
    {
        if (item.Item == null)
            return;
        
        var key = _keySelector(item.Item);
        if (_selectedItems.Contains(key))
            _selectedItems.Remove(key);
        else
            _selectedItems.Add(key);
        IsMultiSelecting = _selectedItems.Any();
    }

    public void SelectAll()
    {
        foreach (var item in Items)
            item.IsSelected = false;
    }

    public void DeSelectAll()
    {
        Items.ForEach(i => i.IsSelected = false);
        SelectedItemsCount = 0;
    }

    public async Task RefreshAsync()
    {
        _paginationHelper.Reset();
        await CallApi();
        await OnDataRefresh();
    }

    public SortField SortField
    {
        get => _sortField;
        set
        {
            if (_sortField.Equals(value))
                return;
            _sortField = value;
            _onReRender();
        }
    }
    
    public string Query
    {
        get => _query;
        set
        {
            if (_query.Equals(value, StringComparison.OrdinalIgnoreCase))
                return;
            _query = value;
            _onReRender();
        }
    }
}