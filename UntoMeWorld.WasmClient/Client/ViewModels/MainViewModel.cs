using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using static System.Console;

namespace UntoMeWorld.WasmClient.Client.ViewModels;

public class MainViewModel
{
    private readonly IDictionary<string, Action> _fabClickListenersByPage = new Dictionary<string, Action>();
    private string CurrentPage { get; set; }


    public MainViewModel(NavigationManager navigation) 
    {
        navigation.LocationChanged += NavigationOnLocationChanged;
    }

    private void NavigationOnLocationChanged(object _, LocationChangedEventArgs e)
        => CurrentPage = string.IsNullOrEmpty(e.Location) ? "" : e.Location;

    public void RegisterFabClickListener(string page, Action listener)
    {
        if (string.IsNullOrEmpty(page))
            return;
        var key = page.ToLower().Trim();
        WriteLine("Registering fab listener for page => " + key);
        _fabClickListenersByPage[key] = listener;
    }

    public void OnFabClicked()
    {
        if (string.IsNullOrEmpty(CurrentPage))
            return;
        var currentPage = CurrentPage.ToLower();
        WriteLine("Dispatching fab listener for page => " + currentPage);
        var listenerKey = _fabClickListenersByPage.Keys.FirstOrDefault(page => currentPage.EndsWith(page));
        if (!string.IsNullOrEmpty(listenerKey))
            _fabClickListenersByPage[listenerKey]();
    }
}