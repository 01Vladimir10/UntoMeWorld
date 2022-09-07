using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UntoMeWorld.Application.Extensions;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Common.Helpers;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Options;

namespace UntoMeWorld.WasmClient.Server.Services;

public class RolesService : GenericSecurityService<Role>, IRolesService
{
    private readonly IRoleStore _store;
    private readonly RolesServiceOptions _options;
    private readonly CacheHelper<Dictionary<string, Permission>, string> _permissionsByRoles;

    public RolesService(IRoleStore store, IMemoryCache memoryCache, IOptions<RolesServiceOptions> options)
        : base(store,
            new CacheHelper<Role, string>(memoryCache, "Roles__",
                TimeSpan.FromSeconds(options.Value.CacheLifetimeInSeconds)), options.Value.EnableCaching)
    {
        _store = store;
        _options = options.Value;
        _permissionsByRoles = new CacheHelper<Dictionary<string, Permission>, string>(memoryCache,
            "PermissionsByRoles__", TimeSpan.FromSeconds(options.Value.CacheLifetimeInSeconds));
    }

    public Task<List<Role>> GetRoleByUser(string userId)
        => _store.GetByUser(userId);
    public Task<Role> GetByRoleName(string roleName)
        => _store.GetByRoleName(roleName);

    public Task<List<Role>> GetByRoleName(params string[] roleNames)
        => _store.GetByRoleName(roleNames);

    public async Task<Dictionary<string, Permission>> GetEffectivePermissionByRole(IEnumerable<string> roles)
    {
        var rolesList = roles.ToList();
        return _options.EnableCaching
            ? await _permissionsByRoles.Get(BuildRolesCacheKey(rolesList), () => _GetEffectivePermissionsByRoles(rolesList))
            : await _GetEffectivePermissionsByRoles(rolesList);
    }

    private async Task<Dictionary<string, Permission>> _GetEffectivePermissionsByRoles(IEnumerable<string> roleIds)
        => GetEffectivePermissionsByRoles(await _store.GetByRoleName(roleIds));

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
                effectivePermissions[entry.Key] = MergePermissions(entry.Value, _options.PermissionSelectionMode);
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

    private static string BuildRolesCacheKey(IEnumerable<string> roles)
        => string.Join("_", roles.OrderBy(r => r));
}