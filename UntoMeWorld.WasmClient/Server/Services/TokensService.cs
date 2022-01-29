using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Model;
using UntoMeWorld.WasmClient.Server.Services.Abstractions;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Services;

public class TokensService : GenericSecurityService<Token>, ITokensService
{
    private readonly ITokenStore _tokenStore;
    private const string TokenHashesPrefix = "TokenHashes__";

    public TokensService(ITokenStore store, ICacheService cache) :
        base(store, cache, true, "Tokens__", TimeSpan.FromMinutes(15))
    {
        _tokenStore = store;
    }

    public async Task EnableToken(params string[] tokenIds)
    {
        await _tokenStore.EnableToken(tokenIds);
        if (!EnableCache)
            return;
        foreach (var tokenId in tokenIds)
        {
            var cachedToken = await Cache.Get<Token>(tokenId);
            if (cachedToken == null)
                continue;
            cachedToken.IsDisabled = false;
            Cache.SetEntry(CachePrefix + cachedToken.Id, BuildCacheEntry(cachedToken));
        }
    }

    public async Task DisableToken(params string[] tokenIds)
    {
        await _tokenStore.DisableToken(tokenIds);
        if (!EnableCache)
            return;
        foreach (var tokenId in tokenIds)
        {
            var cachedToken = await Cache.Get<Token>(tokenId);
            if (cachedToken == null)
                continue;
            cachedToken.IsDisabled = true;
            Cache.SetEntry(CachePrefix + cachedToken.Id, BuildCacheEntry(cachedToken));
        }
    }

    // This query is not cached as it wont be 
    // invoked very often.
    public Task<List<Token>> GetByUser(string userId)
        => _tokenStore.GetByUser(userId);

    public Task<Token> GetTokenByHash(string hash)
        => _GetTokenByHash(hash);

    /// <summary>
    /// Gets the tokens from the cache by hash and updates it if expired.
    /// It maintains an index where it maps tokens hashes to token ids in the cache
    /// so it can use the id of the token to get them from the cache that is used
    /// by the rest of the service.
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    private async Task<Token> _GetTokenByHash(string hash)
    {
        if (!EnableCache)
            return await _tokenStore.GetByHash(hash);
        var tokenId = await Cache.Get<string>(TokenHashesPrefix + hash);
        
        // The has is indexed in the tokens hashes to ids index.
        // get it from the ids to tokens cache and fetch it from
        // the database if expired.
        if (!string.IsNullOrEmpty(tokenId))
            return await Cache.GetEntry(tokenId,
                async () => new CacheEntry<Token>
                    { Data = await _tokenStore.GetByHash(hash), LifeSpan = CacheLifespan });
        
        // The token has is not indexed in the cache.
        // Get the token from the database using its hash.
        var token = await _tokenStore.GetByHash(hash);
        // Add it to the hashes index.
        Cache.SetEntry(TokenHashesPrefix + hash,  new CacheEntry<string> {Data = tokenId, LifeSpan = CacheLifespan});
        // Add the token to the actual cache, where its indexed by its id.
        Cache.SetEntry(CachePrefix + token.Id, BuildCacheEntry(token));
        return token;

    }
}