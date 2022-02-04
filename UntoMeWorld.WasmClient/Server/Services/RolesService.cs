using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Helpers;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Options;

namespace UntoMeWorld.WasmClient.Server.Services;

public class RolesService : GenericSecurityService<Role>, IRolesService
{
    private readonly IRoleStore _store;
    public RolesService(IRoleStore store, IMemoryCache memoryCache, IOptions<RolesServiceOptions> options) 
        : base(store,new CacheHelper<Role, string>(memoryCache, "Roles__", TimeSpan.FromSeconds(options.Value.CacheLifetimeInSeconds)), options.Value.EnableCaching)
    {
        _store = store;
    }
    public Task<List<Role>> GetRoleByUser(string userId)
        => _store.GetByUser(userId);

    public Task<Role> GetByRoleName(string roleName)
        => _store.GetByRoleName(roleName);
    public Task<IDictionary<string, Role>> GetByRoleName(params string[] roleNames)
        => _store.GetByRoleName(roleNames);
}