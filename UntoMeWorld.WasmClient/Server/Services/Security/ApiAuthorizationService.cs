using System.Security.Cryptography;
using System.Text;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services.Security;

public class ApiAuthorizationService : IApiAuthorizationService
{
    private readonly IUserService _users;
    private readonly IRolesService _roles;
    private readonly ITokensService _tokens;

    public ApiAuthorizationService(ITokensService tokens, IRolesService roles, IUserService users)
    {
        _tokens = tokens;
        _roles = roles;
        _users = users;
    }
    
    public async Task<bool> ValidateUserAuthenticatedRequest(string userId, string controller, string action)
    {
        var user = await GetUser(userId);
        if (user == null || user.IsDisabled || user.IsDeleted)
            return false;
        var permissions = await GetUserApiPermissions(user.RoleIds);
        return ValidateActionOnController(permissions, controller, action);
    }

    public async Task<bool> ValidateTokenAuthenticatedRequest(string token, string controller, string action)
    {
        var hash = GetTokenHash(token);
        var tokenInfo = await GetToken(hash);
        if (tokenInfo == null || tokenInfo.IsDisabled || tokenInfo.ExpiresOn != default && DateTime.Now > tokenInfo.ExpiresOn)
            return false;
        return await ValidateUserAuthenticatedRequest(tokenInfo.UserId, controller, action);
    }

    #region Caching

    private async Task<Dictionary<string, Permission>> GetRoleApiPermissions(string roleId)
    {
        var role = await _roles.Get(roleId);
        return role.Permissions
            .Where(p => p.ResourceType == ResourceTypes.ApiEndPoint)
            .ToDictionary(p => p.Resource.ToUpper(), p => p);
    }

    private async Task<IDictionary<string, IDictionary<string, Permission>>> GetUserApiPermissions(IEnumerable<string> roleIds)
    {
        var permissions = new Dictionary<string, IDictionary<string, Permission>>();
        foreach (var roleId in roleIds)
        {
            var permission = await GetRoleApiPermissions(roleId);
            permissions[roleId] = permission;
        }
        return permissions;
    }

    private Task<AppUser> GetUser(string userId)
        => _users.Get(userId);

    private Task<Token> GetToken(string hash)
        => _tokens.GetTokenByHash(hash);
    
    #endregion
    #region PermissionsEvaluators
    // Apply the permissions of the most permissive roles.
    private static bool ValidateActionOnController(IDictionary<string, IDictionary<string, Permission>> rolesPermissions,
        string controller, string action)
    {
        foreach (var (key, value) in rolesPermissions)
        {
            if (!ValidateActionOnController(value, controller, action)) continue;
#if DEBUG
            Console.WriteLine($"Request {controller}/{action} authorized by role: {key}");       
#endif
            return true;
        }
        return false;
    }

    // Apply the permissions of the most specific resource.
    private static bool ValidateActionOnController(IDictionary<string, Permission> permissions, string controller, string action)
    {
        var controllerKey = controller.ToUpper();
        if (!permissions.ContainsKey(controllerKey) && !permissions.ContainsKey("*"))
            return false;
        return permissions.ContainsKey(controllerKey)
            ? ValidatePermission(permissions[controllerKey], action)
            : ValidatePermission(permissions["*"], action);
    }
    private static string GetTokenHash(string token)
    {
        var data = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(token));

        var sBuilder = new StringBuilder();
        foreach (var t in data)
            sBuilder.Append(t.ToString("x2"));
        return sBuilder.ToString();
    }
    private static bool ValidatePermission(Permission permission, string action)
    {
        switch (action)
        {
            case "Add":
            case "BulkInsert":
                return permission.Add;
            case "All":
            case "QueryDeletedElements":
                return permission.Read;
            case "Delete":
            case "BulkDelete":
                return permission.Delete;
            case "Update":
            case "BulkUpdate":
                return permission.Update;
            case "Restore":
                return permission.Restore;
            case "PermanentlyDelete":
                return permission.Delete;
            default:
                return false;
        }
    }
    #endregion
}