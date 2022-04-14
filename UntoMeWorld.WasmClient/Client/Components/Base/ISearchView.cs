using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public interface ISearchView
{
    [Parameter]
    public string CssClass { get; set; }
    [Parameter]
    public int MaxSuggestions { get; set; }
    
    [Parameter]
    public string Query { get; set; }
    
    [Parameter]
    public bool EnableSuggestions { get; set; }
    
    [Parameter]
    public string HistoryCollectionKey { get; set; }
    
    [Parameter]
    public string Placeholder { get; set; }
    
    [Parameter]
    public int MinQueryLength { get; set; }
    
    [Parameter]
    public Func<string, Task> OnSearch { get; set; }
    
    [Parameter]
    public Func<string, Task<IEnumerable<string>>> QuerySuggestionProvider { get; set; }
}