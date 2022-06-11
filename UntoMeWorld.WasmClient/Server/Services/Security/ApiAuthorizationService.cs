using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Extensions;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Model.Abstractions;
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
        IOptions<AuthorizationServiceOptions> options, IMemoryCache cache)
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
        var permissions = await GetEffectivePermissions(user);
        return ValidateActionOnController(permissions, controller, action);
    }

    public async Task<bool> ValidateTokenAuthenticatedRequest(string jwtToken, string controller, string action)
    {
        if (string.IsNullOrEmpty(jwtToken) || !await _tokens.Validate(jwtToken))
            return false;
        var token = _tokens.Read(jwtToken);
        var permissions = await GetEffectivePermissions(token);
        return ValidateActionOnController(permissions, controller, action);
    }

    #region PermissionsEvaluators
    private bool ValidateActionOnController(IDictionary<string, Permission> permissions, string controller, string action)
    {
        Permission permission;
        if (permissions.ContainsKey(controller))
            permission = permissions[controller];
        else if (permissions.ContainsKey(controller.ToUpper()))
            permission = permissions[controller.ToUpper()];
        else
            return false;
        return ValidatePermission(permission, action);
    }
    private async Task<Dictionary<string, Permission>> GetEffectivePermissions(Token token)
    {
        var roles = new List<Role>();
        foreach (var role in token.Roles)
        {
            roles.Add(await _roles.GetByRoleName(role));
        }
        return GetEffectivePermissionsByRoles(roles);
    }
    private async Task<Dictionary<string, Permission>> GetEffectivePermissions(IModel user)
        => GetEffectivePermissionsByRoles(await _roles.GetRoleByUser(user.Id));
    private Dictionary<string, Permission> GetEffectivePermissionsByRoles(IReadOnlyList<Role> roles)
    {
        if (roles.Count <= 1)
            return roles[0].Permissions.ToDictionary(p => p.Resource);
        
        var permissionsByResource = new Dictionary<string, List<Permission>>();
        foreach (var permission in roles.SelectMany(role => role.Permissions.Where(p =>
                     p.ResourceType.In(ResourceTypes.ApiEndPoint, ResourceTypes.Wildcard))))
        {
            if (!permissionsByResource.ContainsKey(permission.Resource))
                permissionsByResource[permission.Resource] = new List<Permission>();
            // Add it to the list.
            permissionsByResource[permission.Resource].Add(permission);
        }

        var effectivePermissions = new Dictionary<string, Permission>();
        foreach (var entry in permissionsByResource)
        {
            if (entry.Value.Count <= 1)
            {
                effectivePermissions[entry.Key] = entry.Value[0];
            }
            else
            {
                effectivePermissions[entry.Key] = MergePermissions(entry.Value, PermissionSelectionMode);
            }
        }
        return effectivePermissions;
    }
    private static Permission MergePermissions(IReadOnlyCollection<Permission> permissions, string selectionMode)
    {
        var permission = new Permission
        {
            Resource = permissions.First().Resource,
            ResourceType = permissions.First().ResourceType,
        };
        switch (selectionMode)
        {
            case PermissionSelectionModes.LeastPermissive:
                permission.Add = permissions.All(p => p.Add);
                permission.Read = permissions.All(p => p.Read);
                permission.Restore = permissions.All(p => p.Restore);
                permission.Update = permissions.All(p => p.Update);
                permission.Delete = permissions.All(p => p.Delete);
                permission.Purge = permissions.All(p => p.Purge);
                permission.Special = permissions.All(p => p.Special);
                break;
            default:
                permission.Add = permissions.Any(p => p.Add);
                permission.Read = permissions.Any(p => p.Read);
                permission.Restore = permissions.Any(p => p.Restore);
                permission.Update = permissions.Any(p => p.Update);
                permission.Delete = permissions.Any(p => p.Delete);
                permission.Purge = permissions.Any(p => p.Purge);
                permission.Special = permissions.Any(p => p.Special);
                break;
        }
        return permission;
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