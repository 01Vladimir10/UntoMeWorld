using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Domain.Stores
{
    public interface IUserStore : IStore<AppUser>
    {
        public Task Disable(params string[] userIds);
        public Task Enable(params string[] userIds);
        public Task<AppUser> GetByThirdPartyUserId(string provider, string providerUserId);
    }
}