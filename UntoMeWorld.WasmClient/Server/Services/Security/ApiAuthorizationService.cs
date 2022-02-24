using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Options;
using UntoMeWorld.WasmClient.Shared.Errors;

namespace UntoMeWorld.WasmClient.Server.Services.Security;

public class ApiAuthorizationService : IApiAuthorizationService
{
    private readonly IUserService _users;
    private readonly IRolesService _roles;
    private readonly ITokensService _tokens;
    private readonly AuthorizationServiceOptions _options;
    private IDictionary<string, PermissionType> _permissionsDictionary;

    private string RoleSelectionMode => string.IsNullOrEmpty(_options.RoleSelectionMode)
        ? RoleSelectionModes.MostPermissive
        : _options.RoleSelectionMode;

    private string PermissionSelectionMode => string.IsNullOrEmpty(_options.PermissionSelectionMode)
        ? PermissionSelectionModes.MostSpecific
        : _options.PermissionSelectionMode;

    public ApiAuthorizationService(ITokensService tokens, IRolesService roles, IUserService users,
        IOptions<AuthorizationServiceOptions> options)
    {
        _tokens = tokens;
        _roles = roles;
        _users = users;
        _options = options?.Value;
        if (options == null)
            throw new InvalidServiceConfigurationError("the AuthorizationServiceOptions cannot be null");
        BindProperties();
    }

    private void BindProperties()
    {
        _permissionsDictionary = new Dictionary<string, PermissionType>();
        _options.SpecialActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Special);
        _options.DeleteActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Delete);
        _options.PurgeActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Purge);
        _options.UpdateActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Update);
        _options.AddActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Add);
        _options.RestoreActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Restore);
        _options.ReadActions?.ForEach(a => _permissionsDictionary[a] = PermissionType.Read);
    }

    public async Task<bool> ValidateUserAuthenticatedRequest(AppUser user, string controller, string action)
    {
        if (user == null || user.IsDisabled || user.IsDeleted || await _users.IsDisabled(user.Id))
            return false;
        var permissions = (await GetRolesPermissions(user.Roles))?.ToList() ?? new List<IDictionary<string, Permission>>();
        return permissions.Any() && ValidateActionOnController(permissions, controller, action);
    }

    public async Task<bool> ValidateTokenAuthenticatedRequest(string jwtToken, string controller, string action)
    {
        if (string.IsNullOrEmpty(jwtToken) || !await _tokens.Validate(jwtToken))
            return false;
        var token = _tokens.Read(jwtToken);
        var permissions = (await GetRolesPermissions(token.Roles))?.ToList() ?? new List<IDictionary<string, Permission>>();
        return permissions.Any() && ValidateActionOnController(permissions, controller, action);
    }

    #region Caching

    private async Task<Dictionary<string, Permission>> GetRoleApiPermissions(string roleName)
    {
        var role = await _roles.GetByRoleName(roleName);
        return
            role == null
                ? new Dictionary<string, Permission>()
                : role.Permissions
                    .Where(p => p.ResourceType == ResourceTypes.ApiEndPoint)
                    .ToDictionary(p => p.Resource.ToUpper(), p => p);
    }

    private async Task<IEnumerable<IDictionary<string, Permission>>> GetRolesPermissions(IEnumerable<string> roleIds)
    {
        var permissions = new List<IDictionary<string, Permission>>();
        foreach (var roleId in roleIds)
        {
            permissions.Add(await GetRoleApiPermissions(roleId));
        }

        return permissions;
    }

    #endregion

    #region PermissionsEvaluators

    // Apply the permissions of the most permissive roles.
    private bool ValidateActionOnController(IEnumerable<IDictionary<string, Permission>> rolesPermissions,
        string controller, string action)
    {
        return RoleSelectionMode == RoleSelectionModes.LeastPermissive
            ? rolesPermissions.All(p => ValidateEffectivePermission(p, controller, action))
            : rolesPermissions.Any(p => ValidateEffectivePermission(p, controller, action));
    }

    private bool ValidateEffectivePermission(IDictionary<string, Permission> permissions, string controller,
        string action)
    {
        var effectivePermissions = permissions.Where(p =>
                string.Equals(p.Key, controller, StringComparison.InvariantCultureIgnoreCase) || p.Key == "*")
            .Select(p => p.Value);

        return PermissionSelectionMode switch
        {
            PermissionSelectionModes.LeastPermissive => ValidateByLeastPermissivePermission(effectivePermissions,
                action),
            PermissionSelectionModes.MostPermissive => ValidateByMostPermissivePermission(effectivePermissions, action),
            _ => ValidateByMostSpecificPermission(effectivePermissions, action)
        };
    }

    private bool ValidateByMostPermissivePermission(IEnumerable<Permission> permissions, string action)
    {
        return permissions.Any(permission => ValidatePermission(permission, action));
    }

    private bool ValidateByLeastPermissivePermission(IEnumerable<Permission> permissions, string action)
    {
        return permissions.All(permission => ValidatePermission(permission, action));
    }

    private bool ValidateByMostSpecificPermission(IEnumerable<Permission> permissions, string action)
    {
        var permissionsList = permissions.ToList();
        if (!permissionsList.Any())
            return false;
        return ValidatePermission(
            permissionsList.Count == 1
                ? permissionsList.First()
                : permissionsList.FirstOrDefault(p => p.Resource != "*"), action);
    }


    private bool ValidatePermission(Permission permission, string action)
    {
        if (!_permissionsDictionary.ContainsKey(action))
            return false;
        switch (_permissionsDictionary[action])
        {
            case PermissionType.Add: return permission.Add;
            case PermissionType.Delete:
                return permission.Delete;
            case PermissionType.Update:
                return permission.Update;
            case PermissionType.Read: return permission.Read;
            case PermissionType.Restore: return permission.Restore;
            case PermissionType.Purge: return permission.Purge;
            case PermissionType.Special: return permission.Special;
            case PermissionType.Unknown:
            default:
                return false;
        }
    }

    #endregion
}

internal enum PermissionType
{
    Add,
    Delete,
    Update,
    Read,
    Restore,
    Purge,
    Special,
    Unknown
}