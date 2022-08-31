﻿using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Security;
using UntoMeWorld.WasmClient.Client.Data.Common;
using UntoMeWorld.WasmClient.Client.Utils;
using UntoMeWorld.WasmClient.Client.Utils.Extensions;

namespace UntoMeWorld.WasmClient.Client.Services.Security;

public class ApiAuthorizationService : IAuthorizationProviderService
{
    public IDictionary<ApiResource, Permission> CurrentUserPermissions { get; set; }
    private readonly HttpClient _httpClient;
    private bool _isInitialized;

    public ApiAuthorizationService(IHttpClientFactory client)
    {
        _httpClient = client.CreateClient("UntoMeWorld.WasmClient.ServerAPI");
    }

    public async Task<bool> ChallengeAsync(ApiResource apiResource, PermissionType requiredPermission)
    {
        if (!_isInitialized || CurrentUserPermissions == null)
        {
            _isInitialized = true;
            CurrentUserPermissions = await GetCurrentUsersPermission();
        }

        if (CurrentUserPermissions.ContainsKey(apiResource))
            return ChallengePermissions(CurrentUserPermissions[apiResource], requiredPermission);
        
        return CurrentUserPermissions.ContainsKey(ApiResource.Wildcard) &&
               ChallengePermissions(CurrentUserPermissions[ApiResource.Wildcard], requiredPermission);
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
        try
        {
            var permissions =
                await _httpClient.GetJsonAsync<Dictionary<string, Permission>>(
                    ApiRoutes.Roles.GetCurrentUserPermissions);
        
            var result = new Dictionary<ApiResource, Permission>();

            foreach (var resource in permissions.Keys.Where(resource => resource != null))
            {
                if (Enum.TryParse<ApiResource>(resource, out var apiResource))
                    result[apiResource] = permissions[resource];
                else
                    result[ApiResource.Wildcard] = permissions[resource];
            }
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}