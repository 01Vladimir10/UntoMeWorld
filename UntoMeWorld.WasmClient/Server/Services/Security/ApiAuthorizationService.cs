﻿using Microsoft.Extensions.Options;
using UntoMeWorld.Application.Errors;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Services.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;

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

    public async Task<bool> ValidateUserAuthenticatedRequest(AppUser user, ApiResource apiResource,
        PermissionType requiredPermission)
    {
        if (user == null || user.IsDisabled || user.IsDeleted || await _users.IsDisabled(user.Id))
            return false;
        var permissions = await _roles.GetEffectivePermissionByRole(user.Roles);
        return ValidateActionOnController(permissions, apiResource, requiredPermission);
    }

    public async Task<bool> ValidateTokenAuthenticatedRequest(string jwtToken, ApiResource apiResource,
        PermissionType requiredPermission)
    {
        if (string.IsNullOrEmpty(jwtToken) || !await _tokens.Validate(jwtToken))
            return false;
        var token = _tokens.Read(jwtToken);
        var permissions = await _roles.GetEffectivePermissionByRole(token?.Roles ?? new List<string>());
        return ValidateActionOnController(permissions, apiResource, requiredPermission);
    }

    #region PermissionsEvaluators

    private bool ValidateActionOnController(IDictionary<string, Permission> permissions, ApiResource apiResource,
        PermissionType permissionType)
    {
        Permission permission;
        if (permissions.ContainsKey(apiResource.ToString()))
            permission = permissions[apiResource.ToString()];
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