using Microsoft.Extensions.Caching.Memory;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Helpers;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class RolesService : GenericSecurityService<Role>, IRolesService
{
    private readonly IRoleStore _store;
    public RolesService(IRoleStore store, IMemoryCache memoryCache) 
        : base(store, new CacheHelper<Role, string>(memoryCache, "Roles__", TimeSpan.FromMinutes(15)))
    {
        _store = store;
    }

    public Task<List<Role>> GetRoleByUser(string userId)
        => _store.GetByUser(userId);
}