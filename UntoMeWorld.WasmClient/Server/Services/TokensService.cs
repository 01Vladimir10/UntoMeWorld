using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class TokensService : GenericSecurityService<Token>, ITokensService
{
    private readonly ITokenStore _tokenStore;
    public TokensService(ITokenStore store) : base(store)
    {
        _tokenStore = store;
    }

    public Task EnableToken(params string[] hashes)
        => _tokenStore.EnableToken(hashes);

    public Task DisableToken(params string[] hashes)
        => _tokenStore.EnableToken(hashes);

    public Task<List<Token>> GetByUser(string userId)
        => _tokenStore.GetByUser(userId);

    public Task<Token> GetIfValid(string hash)
        => _tokenStore.GetIfValid(hash);
}