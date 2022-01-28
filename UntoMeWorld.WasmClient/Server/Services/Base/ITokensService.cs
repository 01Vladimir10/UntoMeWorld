using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface ITokensService : ISecurityService<Token>
{
    public Task EnableToken(params string[] hashes);
    public Task DisableToken(params string[] hashes);
    public Task<List<Token>> GetByUser(string userId);
    public Task<Token> GetIfValid(string hash);
}