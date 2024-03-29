﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using UntoMeWorld.WasmClient.Client.Components.Base;

namespace UntoMeWorld.WasmClient.Client.Components.SearchControls;

public class SearchViewBase : BaseSearchView
{
    [Inject] public ILocalStorageService? LocalStorageService { get; set; }
    private bool _hasHistoryBeenLoaded;
    private HashSet<string> _history = new();
    protected List<SearchSuggestion> Suggestions { get; private set; } = new();

    protected async Task OnSuggestionClicked(string? suggestion)
    {
        if (string.IsNullOrEmpty(suggestion))
            return;
        await UpdateValueAsync(suggestion);
    }

    protected Task OnClearSearch()
    {
        IsSearching = false;
        return UpdateValueAsync(string.Empty);
    }

    protected async Task OnQueryChanged(ChangeEventArgs args)
    {
        var query = args.Value?.ToString() ?? string.Empty;
        if (EnableSuggestions)
            await UpdateSuggestions(query);
    }

    private async Task UpdateSuggestions(string query)
    {
        var suggestions = new List<SearchSuggestion>();
        if (EnableSuggestions)
            suggestions.AddRange(await GetSearchHistory(query));

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
                    : s.Suggestion?.Replace(query, $"<b>{query}</b>", StringComparison.InvariantCultureIgnoreCase),
                Id = i++, IsFromHistory = s.IsFromHistory
            }).ToList();
    }


    private async Task LoadHistory()
    {
        if (_hasHistoryBeenLoaded)
            return;
        if (LocalStorageService != null)
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
        if (LocalStorageService != null)
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

    protected override bool AreEqual(string? currentValue, string? newValue)
        => string.Equals(currentValue, newValue, StringComparison.OrdinalIgnoreCase);
    protected override async Task UpdateValueAsync(string? value)
    {
        if (EnableSuggestions && !string.IsNullOrEmpty(value))
        {
            await SaveToHistory(value);
        }
        await base.UpdateValueAsync(value);
    }
}

public class SearchSuggestion
{
    public string? HtmlSuggestion { get; set; }
    public string? Suggestion { get; set; }
    public bool IsFromHistory { get; set; }
    public int Id { get; set; }
}