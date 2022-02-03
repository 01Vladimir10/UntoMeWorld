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

    public async Task<bool> IsDisabled(string tokenId)
    {
        var token = EnableCache ?
            await Cache.Get(tokenId, () => _tokenStore.Get(tokenId)) :
            await _tokenStore.Get(tokenId);
        return token?.IsDisabled ?? true;
    }
}