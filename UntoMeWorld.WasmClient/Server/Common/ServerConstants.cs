namespace UntoMeWorld.WasmClient.Server.Common;

public static class ServerConstants
{
    public const string HeaderToken = "ApiToken";
}

public static class PermissionSelectionModes
{
    public const string MostPermissive = "MOST_PERMISSIVE";
    public const string LeastPermissive = "LEAST_PERMISSIVE";
    public const string MostSpecific = "MOST_SPECIFIC";
}
public static class RoleSelectionModes
{
    public const string MostPermissive = "MOST_PERMISSIVE";
    public const string LeastPermissive = "LEAST_PERMISSIVE";
}