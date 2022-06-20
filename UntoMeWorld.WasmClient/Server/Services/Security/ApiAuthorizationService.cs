using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
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

    public ApiAuthorizationService(ITokensService tokens, IRolesService roles, IUserService users,
        IOptions<AuthorizationServiceOptions> options)
    {
        _tokens = tokens;
        _roles = roles;
        _users = users;
        if (options == null)
            throw new InvalidServiceConfigurationError("the AuthorizationServiceOptions cannot be null");
    }

    public async Task<bool> ValidateUserAuthenticatedRequest(AppUser user, ResourceType resource,
        PermissionType requiredPermission)
    {
        if (user == null || user.IsDisabled || user.IsDeleted || await _users.IsDisabled(user.Id))
            return false;
        var permissions = await _roles.GetEffectivePermissionByRole(user.Roles);
        return ValidateActionOnController(permissions, resource, requiredPermission);
    }

    public async Task<bool> ValidateTokenAuthenticatedRequest(string jwtToken, ResourceType resource,
        PermissionType requiredPermission)
    {
        if (string.IsNullOrEmpty(jwtToken) || !await _tokens.Validate(jwtToken))
            return false;
        var token = _tokens.Read(jwtToken);
        var permissions = await _roles.GetEffectivePermissionByRole(token.Roles);
        return ValidateActionOnController(permissions, resource, requiredPermission);
    }

    #region PermissionsEvaluators

    private bool ValidateActionOnController(IDictionary<string, Permission> permissions, ResourceType resource,
        PermissionType permissionType)
    {
        Permission permission;
        if (permissions.ContainsKey(resource.ToString()))
            permission = permissions[resource.ToString()];
        else if (permissions.ContainsKey("*"))
            permission = permissions["*"];
        else
            return false;
        return ValidatePermission(permission, permissionType);
    }

    private bool ValidatePermission(Permission permission, PermissionType requiredPermission)
    {
        switch (requiredPermission)
        {
            case PermissionType.Add: return permission.Add;
            case PermissionType.Delete: return permission.Delete;
            case PermissionType.Update: return permission.Update;
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