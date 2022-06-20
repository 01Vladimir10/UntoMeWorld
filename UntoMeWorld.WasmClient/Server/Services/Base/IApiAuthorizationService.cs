using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Common;

namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface IApiAuthorizationService
{
    public Task<bool> ValidateUserAuthenticatedRequest(AppUser userId, ResourceType resource, PermissionType requiredPermission);
    public Task<bool> ValidateTokenAuthenticatedRequest(string token, ResourceType resource, PermissionType permission);
}