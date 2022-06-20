namespace UntoMeWorld.WasmClient.Server.Common;

public static class ServerConstants
{
    public const string HeaderToken = "ApiToken";
}

public static class PermissionSelectionModes
{
    public const string MostPermissive = "MostPermissive";
    public const string LeastPermissive = "LeastPermissive";
    public const string MostSpecific = "MostSpecific";
}
public static class RoleSelectionModes
{
    public const string MostPermissive = "MostPermissive";
    public const string LeastPermissive = "LeastPermissive";
}

public static class EnvironmentVariables
{
    public const string JwtSecret = "UW_JWT_SECRET";
    public const string DatabaseConnectionString = "UW_DB_CONNECTION_STRING";
    public const string DatabaseName = "UW_DB_NAME";
}

public static class AuthPolicies
{
    public const string UsersOnly = "UserAuthenticationOnly";
    public const string TokenAuthenticationOnly = "TokenAuthenticationOnly";
}

public enum ResourceType
{
    Children,
    Pastors,
    Churches,
    Roles,
    Tokens,
    Unknown
}