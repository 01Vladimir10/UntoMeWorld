using Microsoft.Extensions.Caching.Memory;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Helpers;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class TokensService : GenericSecurityService<Token>, ITokensService
{
    private readonly ITokenStore _tokenStore;
    public TokensService(ITokenStore store, IMemoryCache cache) 
        : base(store, new CacheHelper<Token, string>(cache, "Tokens__", TimeSpan.FromMinutes(15)))
    {
        Cache.CreateIndex(nameof(Token.Hash), t => t.Hash);
        _tokenStore = store;
    }

    public async Task EnableToken(params string[] tokenIds)
    {
        await _tokenStore.EnableToken(tokenIds);
        if (!EnableCache) return;
        Cache.UpdateIfPresent((item, _) =>
        {
            item.IsDisabled = false;
            return item;
        }, tokenIds);
    }

    public async Task DisableToken(params string[] tokenIds)
    {
        await _tokenStore.DisableToken(tokenIds);
        if (!EnableCache) return;
        Cache.UpdateIfPresent((item, _) =>
        {
            item.IsDisabled = true;
            return item;
        }, tokenIds);
    }

    // This query is not cached as it wont be 
    // invoked very often.
    public Task<List<Token>> GetByUser(string userId)
        => _tokenStore.GetByUser(userId);

    public Task<Token> GetTokenByHash(string hash)
        => EnableCache
            ? Cache.GetByIndexedProperty(nameof(Token.Hash), hash, async () =>
            {
                var token = await _tokenStore.GetByHash(hash);
                return (token.Id, token);
            })
            : _tokenStore.GetByHash(hash);
}