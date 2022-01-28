using System.Collections.Generic;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Domain.Stores
{
    public interface ITokenStore : ISecurityStore<Token>
    {
        public Task<List<Token>> GetByUser(string userId);
        public Task<Token> GetByHash(string hash);
        public Task EnableToken(params string[] tokens);
        public Task DisableToken(params string[] tokens);
        public Task<Token> GetIfValid(string hash);
    }
}