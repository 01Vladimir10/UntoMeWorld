using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class RolesService : GenericSecurityService<Role>, IRolesService
{
    private readonly IRoleStore _store;
    public RolesService(IRoleStore store) : base(store)
    {
        _store = store;
    }

    public Task<List<Role>> GetRoleByUser(string userId)
        => _store.GetByUser(userId);
}