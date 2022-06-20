using System.Security.Claims;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Constants;

namespace UntoMeWorld.WasmClient.Shared.Security.Utils;

public static class AuthExtensions
{
    public static AppUser ToAppUser(this IEnumerable<Claim> claimsArr)
    {
        if (claimsArr == null)
            return null;
        
        var claims = new Dictionary<string, Claim>();
        foreach (var claim in claimsArr)
            claims[claim.Type] = claim;
        
        var userId = claims.GetOrDefault(CustomClaims.UserId)?.Value;
        var roles = claims.GetOrDefault(CustomClaims.Roles)?.Value.Split(",") ?? Array.Empty<string>();
        var isDeleted = claims.GetOrDefault(CustomClaims.IsDeleted)?.Value.ToLower() == "true";
        var isDisabled = claims.GetOrDefault(CustomClaims.IsDisabled)?.Value.ToLower() == "true";
        var name = claims.GetOrDefault(ClaimTypes.Name)?.Value;
        var lastName = claims.GetOrDefault(ClaimTypes.Surname)?.Value;
        var email = claims.GetOrDefault(ClaimTypes.Email)?.Value;
        var thirdPartyId = claims.GetOrDefault(ClaimTypes.NameIdentifier)?.Value;
        var issuer = claims.GetOrDefault(ClaimTypes.NameIdentifier)?.Issuer;
        return new AppUser
        {
            Id = userId,
            Name = name,
            Lastname = lastName,
            Email = email,
            Roles = roles.ToList(),
            IsDeleted = isDeleted,
            IsDisabled = isDisabled,
            AuthProvider = issuer,
            AuthProviderUserId = thirdPartyId
        };
    }

    public static IEnumerable<Claim> ToClaims(this Token token)
    => token == null ? Array.Empty<Claim>() :
        new Claim[]
        {
            new(CustomClaims.TokenId, token.Id),
            new(CustomClaims.Roles, string.Join(",", token.Roles))
        };

    public static AppUser ToAppUser(this ClaimsPrincipal claimsArr)
        => claimsArr.Claims.ToAppUser();
    private static T GetOrDefault<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
        => dictionary.TryGetValue(key, out var value) ? value : default;
}