using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface IRolesService : ISecurityService<Role>
{
    public Task<List<Role>> GetRoleByUser(string userId);
}