using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Server.Common;

namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface IApiAuthorizationService
{
    public Task<bool> ValidateUserAuthenticatedRequest(AppUser userId, ApiResource apiResource, PermissionType requiredPermission);
    public Task<bool> ValidateTokenAuthenticatedRequest(string token, ApiResource apiResource, PermissionType permission);
}