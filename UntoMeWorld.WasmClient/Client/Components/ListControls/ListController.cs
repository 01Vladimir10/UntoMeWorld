using Microsoft.AspNetCore.Components.Web.Virtualization;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Client.Components.Base;
using UntoMeWorld.WasmClient.Client.Data.Model;

namespace UntoMeWorld.WasmClient.Client.Components.ListControls;

public class ListController<TKey, TItem>
{
    private readonly HashSet<TKey> _selectedItems = new();
    private readonly Func<TItem, TKey> _keySelector;
    private readonly PaginationHelper<TItem> _paginationHelper;
    public Func<Task> OnDataRefresh { get; set; } = () => Task.CompletedTask;
    public List<ListItem<TKey, TItem>> Items { get; set; }
    public ItemsProviderDelegate<ListItem<TKey, TItem>> ItemsProvider { get; set; }
    public int SelectedItemsCount { get; set; }
    public bool IsMultiSelecting { get; set; }

    public ListController(Func<TItem, TKey> keySelector, PaginationHelper<TItem> paginationHelper)
    {
        _keySelector = keySelector;
        _paginationHelper = paginationHelper;
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
    public Task SetFilter(QueryFilter filter)
    {
        _paginationHelper.UpdateQueryFilter(filter);
        return Refresh();
    }
    public Task SetSortField(SortField sort)
    {
        _paginationHelper.UpdateSortByField(sort);
        return Refresh();
    }
    private void OnItemSelectionChanged(ListItem<TKey, TItem> item, bool isSelected)
    {
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

    public async Task Refresh()
    {
        _paginationHelper.Reset();
        await CallApi();
        await OnDataRefresh();
    }
}