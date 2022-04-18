using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseListView<TKey, TItem> : ComponentBase, IComponent
{
    public string CssClass { get; set; }
    public bool IsLoading { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsMultiSelecting { get; set; }
    public List<ListItem<TKey, TItem>> Items { get; set; }
    [Parameter] public ItemsProviderDelegate<ListItem<TKey, TItem>> ItemsProvider { get; set; }

#nullable enable
    [Parameter] public RenderFragment? ListHeaders { get; set; }
    [Parameter] public RenderFragment<ListItem<TKey, TItem>>? ItemTemplate { get; set; }
    [Parameter] public RenderFragment? EmptyTemplate { get; set; }
    [Parameter] public RenderFragment? LoadingTemplate { get; set; }
    
    #nullable disable
    public abstract Task Reset();
}