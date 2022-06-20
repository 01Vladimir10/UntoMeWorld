using Microsoft.AspNetCore.Components.Authorization;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;

namespace UntoMeWorld.WasmClient.Client.Security;

public interface IAuthorizationProvider
{
    public AppUser CurrentUser { get; }
    
    public Dictionary<string, Permission> CurrentUserPermissions { get; }
    public bool IsAuthorized(string resource, PermissionType requiredPermission);
    public Task<bool> IsAuthorizedAsync(string resource, PermissionType requiredPermission);
}