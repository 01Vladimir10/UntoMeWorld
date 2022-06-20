namespace UntoMeWorld.WasmClient.Client.Data.Common;

public static class ApiRoutes
{
    public const string ApiRoot = "/api";

    public static class Roles
    {
        public const string Root = $"{ApiRoot}/roles";
        public const string GetCurrentUserPermissions = $"{Root}/Permissions";
        public const string GetCurrentUserRoles = $"{Root}";
        public const string GetRoleById = $"{Root}/";
    }
}