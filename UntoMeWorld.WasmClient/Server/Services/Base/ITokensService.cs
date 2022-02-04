using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Services.Base;

public interface ITokensService
{
    public Task<string> Add(AppUser user, Token token);
    public Task<Token> Get(string tokenId);
    public Task<List<Token>> GetByUser(string userId);
    public Task Delete(params string[] tokenIds);
    public Task Disable(params string[] tokenIds);
    public Task Enable(params string[] tokenIds);
    public Task<bool> Validate(string jwtToken);
    public Token Read(string jwtToken);

}