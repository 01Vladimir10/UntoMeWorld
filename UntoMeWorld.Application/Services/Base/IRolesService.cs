using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Services.Base;

public interface IRolesService : ISecurityService<Role>
{
    public Task<List<Role>> GetRoleByUser(string userId);
    public Task<Role> GetByRoleName(string roleName);
    public Task<List<Role>> GetByRoleName(params string[] roleNames);
    public Task<Dictionary<string, Permission>> GetEffectivePermissionByRole(IEnumerable<string> roles);
}