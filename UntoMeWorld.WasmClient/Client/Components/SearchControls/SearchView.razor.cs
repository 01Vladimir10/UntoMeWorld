using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.SearchControls;

public class SearchViewBase : BaseSearchView
{
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    private bool _hasHistoryBeenLoaded;
    private HashSet<string> _history;
    protected List<SearchSuggestion> Suggestions { get; private set; }

    protected async Task OnSuggestionClicked(string suggestion)
    {
        Query = suggestion;
        await FireSearch();
    }

    protected async Task OnClearSearch()
    {
        IsSearching = false;
        Query = string.Empty;
        await OnSearch(string.Empty);
    }

    protected async Task OnQueryChanged(ChangeEventArgs args)
    {
        IsSearching = true;
        var query = args.Value?.ToString() ?? string.Empty;
        Query = query;
        if (EnableSuggestions)
            await UpdateSuggestions(Query);
    }

    private async Task FireSearch()
    {
        if (Query.Length >= MinQueryLength)
        {
            IsSearching = false;
            await OnSearch(Query);
            if (EnableSuggestions)
                await SaveToHistory(Query);
        }
    }

    protected async Task OnInputKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
            await FireSearch();
    }

    private async Task UpdateSuggestions(string query)
    {
        var suggestions = new List<SearchSuggestion>();
        if (EnableSuggestions)
            suggestions.AddRange(await GetSearchHistory(query));

        if (QuerySuggestionProvider != null)
            suggestions.AddRange(
                (await QuerySuggestionProvider(query)).Select(s => new SearchSuggestion { Suggestion = s }));
        var i = 0;
        Suggestions = suggestions
            .Where(s => !string.IsNullOrEmpty(s.Suggestion) && s.Suggestion.Length > 1)
            .Take(MaxSuggestions)
            .Select(s => new SearchSuggestion
            {
                Suggestion = s.Suggestion,
                HtmlSuggestion = string.IsNullOrEmpty(query) || query.Length < 1
                    ? s.Suggestion
                    : s.Suggestion.Replace(query, $"<b>{query}</b>", StringComparison.InvariantCultureIgnoreCase),
                Id = i++, IsFromHistory = s.IsFromHistory
            }).ToList();
        Query = query;
    }


    private async Task LoadHistory()
    {
        if (_hasHistoryBeenLoaded)
            return;
        _history = await LocalStorageService.GetItemAsync<HashSet<string>>(HistoryCollectionKey) ??
                   new HashSet<string>();
        _hasHistoryBeenLoaded = true;
    }

    private async Task SaveToHistory(string query)
    {
        await LoadHistory();
        if (_history.Contains(query))
            return;

        _history.Add(query);
        await LocalStorageService.SetItemAsync(HistoryCollectionKey, _history);
    }

    private async Task<List<SearchSuggestion>> GetSearchHistory(string query)
    {
        await LoadHistory();
        return (string.IsNullOrEmpty(query)
                ? _history
                : _history
                    .Where(item => item.ToLower().Contains(query.ToLower())))
            .Select(h => new SearchSuggestion
            {
                Suggestion = h,
                IsFromHistory = true
            })
            .ToList();
    }
}

public class SearchSuggestion
{
    public string HtmlSuggestion { get; set; }
    public string Suggestion { get; set; }
    public bool IsFromHistory { get; set; }
    public int Id { get; set; }
}