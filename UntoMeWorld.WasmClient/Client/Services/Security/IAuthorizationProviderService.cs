using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;

namespace UntoMeWorld.WasmClient.Client.Services.Security;

public interface IAuthorizationProviderService : IDisposable
{
    
    public IDictionary<ApiResource, Permission> CurrentUserPermissions { get; set; }
    public Task<bool> ChallengeAsync(ApiResource apiResource, PermissionType requiredPermission);
    public Task<bool> ChallengeAsync(ApiResource apiResource, IEnumerable<PermissionType> requiredPermissions);
}