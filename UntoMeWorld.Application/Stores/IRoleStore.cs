using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Stores
{
    public interface IRoleStore : ISecurityStore<Role>
    {
        public Task<List<Role>> GetByUser(string userId);
        public Task<Role> GetByRoleName(string roleName);
        public Task<List<Role>> GetByRoleName(IEnumerable<string> roleNames);
    }
}