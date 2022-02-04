using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.WasmClient.Server.Common.Helpers;
using UntoMeWorld.WasmClient.Server.Security.Crypto;
using UntoMeWorld.WasmClient.Server.Security.Utils;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Options;
using UntoMeWorld.WasmClient.Shared.Errors;

namespace UntoMeWorld.WasmClient.Server.Services;

public class TokensService :  ITokensService
{
    private readonly IJwtTokenFactory _factory;
    private readonly ITokenStore _tokenStore;
    private readonly CacheHelper<Token, string> _cache;
    private readonly TokenServiceOptions _options;

    public TokensService(IJwtTokenFactory factory, IOptions<TokenServiceOptions> options, IMemoryCache cache, ITokenStore tokenStore)
    {
        _factory = factory;
        _tokenStore = tokenStore;
        _options = options.Value;
        
        if (_options == null || _options.EnableCaching && _options.CacheLifeTimeInSeconds == 0)
            throw new InvalidServiceConfigurationError("please check the configuration provided, make sure that the cache lifetime is different from 0 when enabling cache");
        
        if (_options.EnableCaching)
            _cache = new CacheHelper<Token, string>(cache, "Tokens__", TimeSpan.FromSeconds(_options.CacheLifeTimeInSeconds));
    }
    public async Task<string> Add(AppUser user, Token token)
    {
        // Save the token to the database.
        await _tokenStore.Add(token);
        // Get the token information and give it to the token factory
        // to include it in the JWT token.
        var jwtToken = _factory.GenerateToken(token.ToClaims(), token.ExpiresOn);

        return jwtToken;
    }

    public Task<Token> Get(string tokenId)
        => _tokenStore.Get(tokenId);

    public Task<List<Token>> GetByUser(string userId)
        => _tokenStore.GetByUser(userId);

    public Task Delete(params string[] tokenIds)
        => _tokenStore.Delete(tokenIds);

    public async Task Disable(params string[] tokenIds)
    {
        if (tokenIds == null || tokenIds.Length < 1)
            throw new InvalidParameterError(nameof(tokenIds));
        await _tokenStore.DisableToken(tokenIds);
        if (!_options.EnableCaching)
            return;
        foreach (var tokenId in tokenIds)
            _cache.UpdateIfPresent(tokenId, token =>
            {
                token.IsDisabled = true;
                return token;
            });
    }

    public async Task Enable(params string[] tokenIds)
    {
        if (tokenIds == null || tokenIds.Length < 1)
            throw new InvalidParameterError(nameof(tokenIds));
        await _tokenStore.EnableToken(tokenIds);
        if (!_options.EnableCaching)
            return;
        foreach (var tokenId in tokenIds)
            _cache.UpdateIfPresent(tokenId, token =>
            {
                token.IsDisabled = false;
                return token;
            });
    }

    public async Task<bool> Validate(string jwtToken)
    {
        if (!_factory.ValidateToken(jwtToken))
            return false;
        if (!_options.ValidateTokensInDatabase)
            return true;
        
        var tokenId = Read(jwtToken)?.Id;
        
        if (string.IsNullOrEmpty(tokenId))
            return false;

        var token = _options.EnableCaching
            ? await _cache.Get(tokenId, () => _tokenStore.Get(tokenId))
            : await _tokenStore.Get(tokenId);
        return token is { IsDisabled: false };
    }
    public Token Read(string jwtToken) => _factory.ReadToken(jwtToken).ToToken();
}