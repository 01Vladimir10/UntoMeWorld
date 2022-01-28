using System.Collections.Generic;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Domain.Stores
{
    public interface IRoleStore : ISecurityStore<Role>
    {
        public Task<List<Role>> GetByUser(string userId);
    }
}