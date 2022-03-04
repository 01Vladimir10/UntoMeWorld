using Resources;

namespace UntoMeWorld.WasmClient.Client.Main;

public class PageRoutes
{
    public const string ChildrenRoute = "/children";
    public const string ChurchesRoute = "/churches";
    public const string ReportsRoute = "/reports";
    public const string SettingsRoute = "/settings";

    public static readonly IDictionary<string, string> Routes = new Dictionary<string, string>
    {
        { ChildrenRoute, LangResources.NavigationMenuOptionChildren },
        { ChurchesRoute, LangResources.NavigationMenuOptionChurches },
        { ReportsRoute, LangResources.NavigationMenuOptionReports },
        { SettingsRoute, LangResources.NavigationMenuOptionReports }
    };
}