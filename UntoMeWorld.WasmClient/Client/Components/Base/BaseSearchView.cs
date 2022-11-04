using Microsoft.AspNetCore.Components;

namespace UntoMeWorld.WasmClient.Client.Components.Base;

public abstract class BaseSearchView : BaseInput<string>, IComponent
{
    [Parameter] public string? CssClass { get; set; }
    [Parameter] public int MaxSuggestions { get; set; }
    [Parameter] public bool EnableSuggestions { get; set; }
    [Parameter] public string? HistoryCollectionKey { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public int MinQueryLength { get; set; }

    [Parameter]
    public Func<string, Task<IEnumerable<string>>> QuerySuggestionProvider { get; set; } =
        _ => Task.FromResult(Enumerable.Empty<string>());
    public bool IsSearching { get; set; }
}