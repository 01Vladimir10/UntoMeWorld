using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Client.Data.Common;
using UntoMeWorld.WasmClient.Client.Utils.Common;
using UntoMeWorld.WasmClient.Client.Utils.Extensions;

namespace UntoMeWorld.WasmClient.Client.Services.Security;

public class ApiAuthorizationService : IAuthorizationProviderService
{
    public IDictionary<ApiResource, Permission>? CurrentUserPermissions { get; set; }
    private readonly HttpClient _httpClient;
    private readonly SimpleTaskManager _manager = new();

    public ApiAuthorizationService(IHttpClientFactory client)
    {
        _httpClient = client.CreateClient("UntoMeWorld.WasmClient.ServerAPI");
    }

    private Task Init()
    {
        
        return CurrentUserPermissions != null
            ? Task.CompletedTask
            : _manager.ExecuteTask(async () => CurrentUserPermissions = await GetCurrentUsersPermission());
    }


    public async Task<bool> ChallengeAsync(ApiResource apiResource, PermissionType requiredPermission)
    {
        await Init();
        return Challenge(apiResource, requiredPermission);
    }

    private bool Challenge(ApiResource apiResource, PermissionType requiredPermission)
    {
        if (CurrentUserPermissions!.ContainsKey(apiResource))
            return ChallengePermissions(CurrentUserPermissions[apiResource], requiredPermission);

        return CurrentUserPermissions!.ContainsKey(ApiResource.Wildcard) &&
               ChallengePermissions(CurrentUserPermissions[ApiResource.Wildcard], requiredPermission);
    }

    public async Task<bool> ChallengeAsync(ApiResource apiResource, IEnumerable<PermissionType> requiredPermissions)
    {
        await Init();
        return requiredPermissions.Any(p => Challenge(apiResource, p));
    }

    private static bool ChallengePermissions(Permission permission, PermissionType permissionType)
    {
        return permissionType switch
        {
            PermissionType.Add => permission.Add,
            PermissionType.Delete => permission.Delete,
            PermissionType.Update => permission.Update,
            PermissionType.Read => permission.Read,
            PermissionType.Restore => permission.Restore,
            PermissionType.Purge => permission.Purge,
            PermissionType.Special => permission.Special,
            PermissionType.Unknown => false,
            _ => false
        };
    }


    private async Task<Dictionary<ApiResource, Permission>> GetCurrentUsersPermission()
    {
        var permissions =
            await _httpClient.GetJsonAsync<Dictionary<string, Permission>>(
                ApiRoutes.Roles.GetCurrentUserPermissions);

        var result = new Dictionary<ApiResource, Permission>();

        foreach (var resource in permissions.Keys)
        {
            if (Enum.TryParse<ApiResource>(resource, out var apiResource))
                result[apiResource] = permissions[resource];
            else
                result[ApiResource.Wildcard] = permissions[resource];
        }

        return result;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}