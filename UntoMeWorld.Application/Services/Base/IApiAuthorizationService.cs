using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;

namespace UntoMeWorld.Application.Services.Base;

public interface IApiAuthorizationService
{
    public Task<bool> ValidateUserAuthenticatedRequest(AppUser userId, ApiResource apiResource, PermissionType requiredPermission);
    public Task<bool> ValidateTokenAuthenticatedRequest(string token, ApiResource apiResource, PermissionType permission);
}