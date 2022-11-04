using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseListView<TKey, TItem> : ComponentBase, IComponent
{
    public string? CssClass { get; set; }
    public bool IsLoading { get; set; }
    public bool IsEmpty { get; set; }
    
    public bool IsMultiSelecting { get; set; }
    [Parameter] public ItemsProviderDelegate<ListItem<TKey, TItem>> ItemsProvider { get; set; } = default!;

    [Parameter] public List<ListColumn> Columns { get; set; } = new();
    
    [Parameter] public RenderFragment? ListHeaders { get; set; }
    [Parameter] public RenderFragment? EmptyTemplate { get; set; }
    [Parameter] public RenderFragment? LoadingTemplate { get; set; }
    [Parameter] public RenderFragment<ListCell<TKey, TItem>>? ItemTemplate { get; set; }
    [Parameter] public RenderFragment<TItem>? ExpandableItemContentTemplate { get; set; }
    public abstract Task Refresh();
}

public class ListColumn
{
    public string CssClass { get; } = "";
    public string Header { get; } = "";

    public string HeaderCssClass { get; set; } = "";
    public ListColumn()
    {
        
    }

    public ListColumn(string header, string cssClass)
    {
        Header = header;
        CssClass = cssClass;
    }
 }

public class ListCell<TKey, T>
{
    public TKey? RowKey { get; set; }
    public int ColumnIndex { get; set; }
    public T? Data { get; set; }
}