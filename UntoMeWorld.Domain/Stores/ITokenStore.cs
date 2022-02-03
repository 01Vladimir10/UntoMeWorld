using System.Collections.Generic;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Domain.Stores
{
    public interface ITokenStore : ISecurityStore<Token>
    {
        public Task<List<Token>> GetByUser(string userId);
        public Task EnableToken(params string[] tokenIds);
        public Task DisableToken(params string[] tokenIds);
    }
}