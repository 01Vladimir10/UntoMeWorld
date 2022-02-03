using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Utils;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services.Security;

public class ApiAuthorizationService : IApiAuthorizationService
{
    private readonly IUserService _users;
    private readonly IRolesService _roles;
    private readonly ITokensService _tokens;
    private readonly IJwtTokenFactory _jwtTokens;

    public ApiAuthorizationService(ITokensService tokens, IRolesService roles, IUserService users, IJwtTokenFactory jwtTokens)
    {
        _tokens = tokens;
        _roles = roles;
        _users = users;
        _jwtTokens = jwtTokens;
    }
    
    public async Task<bool> ValidateUserAuthenticatedRequest(AppUser user, string controller, string action)
    {
        if (user == null || user.IsDisabled || user.IsDeleted || await _users.IsDisabled(user.Id))
            return false;
        var permissions = await GetUserApiPermissions(user.Roles);
        return ValidateActionOnController(permissions, controller, action);
    }

    public async Task<bool> ValidateTokenAuthenticatedRequest(string token, string controller, string action)
    {
        var isValid = _jwtTokens.ValidateToken(token);
        if (!isValid)
            return false;

        var jwtToken = _jwtTokens.ReadToken(token);
        if (jwtToken == null)
            return false;
        
        var tokenId = jwtToken.GetTokenId();
        if (string.IsNullOrEmpty(tokenId) || await _tokens.IsDisabled(tokenId))
            return false;
        
        var roles = jwtToken.GetRoles();

        var permissions = await GetUserApiPermissions(roles);
        return ValidateActionOnController(permissions, controller, action);
    }

    #region Caching

    private async Task<Dictionary<string, Permission>> GetRoleApiPermissions(string roleName)
    {
        var role = await _roles.GetByRoleName(roleName);
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

    // Apply the permissions with the most specific resource type
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