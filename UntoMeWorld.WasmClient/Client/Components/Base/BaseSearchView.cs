using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseSearchView : ComponentBase, IComponent
{
    [Parameter] public string CssClass { get; set; }
    [Parameter] public int MaxSuggestions { get; set; }
    [Parameter] public string Query { get; set; }
    [Parameter] public bool EnableSuggestions { get; set; }
    [Parameter] public string HistoryCollectionKey { get; set; }
    [Parameter] public string Placeholder { get; set; }
    [Parameter] public int MinQueryLength { get; set; }
    [Parameter] public Func<string, Task> OnSearch { get; set; }
    [Parameter] public Func<string, Task<IEnumerable<string>>> QuerySuggestionProvider { get; set; }
    public bool IsSearching { get; set; }
    public void AddSearchSubmitListener(Func<string, Task> callback)
    {
        OnSearch = OnSearch == null ? callback : OnSearch + callback;
    }
}