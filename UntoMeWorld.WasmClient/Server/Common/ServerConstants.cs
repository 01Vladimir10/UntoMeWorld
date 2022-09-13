namespace UntoMeWorld.WasmClient.Server.Common;

public static class ServerConstants
{
    public const string HeaderToken = "ApiToken";
}


public static class RoleSelectionModes
{
    public const string MostPermissive = "MostPermissive";
    public const string LeastPermissive = "LeastPermissive";
}

public static class AuthPolicies
{
    public const string UsersOnly = "UserAuthenticationOnly";
    public const string TokenAuthenticationOnly = "TokenAuthenticationOnly";
}