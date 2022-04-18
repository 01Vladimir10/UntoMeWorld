using UntoMeWorld.WasmClient.Client.Data.Model;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public class ListItem<TKey, TItem>
{
    private bool _isSelected;
    public TKey Key { get; set; }
    public TItem Item { get; set; }
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value == _isSelected) return;
            _isSelected = value;
            OnSelectionChanged?.Invoke(this, value);
        }
    }
    public Action<ListItem<TKey, TItem>, bool> OnSelectionChanged { get; set; }
}

public interface IListController<TKey, TItem>
{
    public bool IsLoading { get; set; }
    public Task FilterByKeywords(string query);
    public Task SetSortOrder(SortField sortField);
    public List<ListItem<TKey, TItem>> Items { get; set; }
    public Action<List<ListItem<TKey, TItem>>> OnItemsChanged { get; set; }
    public void AttachSortByDropdown(BaseDropDown<SortField> dropdown);
    public void AttachSearchView(BaseSearchView searchView);
    public void AttachListControlsLayout(BaseListControlsLayout searchView);
    public void AttachListView(BaseListView<TKey, TItem> listViewView);
    public bool IsMultiSelecting { get; set; }
    public Action<int> OnSelectedItemsCountChanged { get; set; }
    public List<TItem> GetSelectedItems();
    public void SelectAll();
    public void DeSelectAll();
    public Task Refresh();
}