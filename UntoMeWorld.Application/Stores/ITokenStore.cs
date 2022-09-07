using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Stores
{
    public interface ITokenStore : ISecurityStore<Token>
    {
        public Task<List<Token>> GetByUser(string userId);
        public Task EnableToken(params string[] tokenIds);
        public Task DisableToken(params string[] tokenIds);
    }
}