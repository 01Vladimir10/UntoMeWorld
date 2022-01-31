using System.Security.Claims;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Constants;

namespace UntoMeWorld.WasmClient.Server.Security.Utils;

public static class AuthExtensions
{
    public static AppUser AsAppUser(this IEnumerable<Claim> claimsArr)
    {
        var claims = claimsArr.ToDictionary(c => c.Type, c => c);
        var userId = claims.GetOrDefault(CustomClaims.UserId).Value;
        var roles = claims.GetOrDefault(CustomClaims.UserId)?.Value.Split(",") ?? Array.Empty<string>();
        var isDeleted = claims.GetOrDefault(CustomClaims.UserId)?.Value.ToLower() == "true";
        var isDisabled = claims.GetOrDefault(CustomClaims.UserId).Value.ToLower() == "true";
        var name = claims.GetOrDefault(ClaimTypes.Name).Value;
        var lastName = claims.GetOrDefault(ClaimTypes.Surname).Value;
        var email = claims.GetOrDefault(ClaimTypes.Email).Value;
        var thirdPartyId = claims.GetOrDefault(ClaimTypes.NameIdentifier).Value;
        var issuer = claims.GetOrDefault(ClaimTypes.NameIdentifier)?.Issuer;
        return new AppUser
        {
            Id = userId,
            Name = name,
            Lastname = lastName,
            Email = email,
            RoleIds = roles.ToList(),
            IsDeleted = isDeleted,
            IsDisabled = isDisabled,
            AuthProvider = issuer,
            AuthProviderUserId = thirdPartyId
        };
    }

    private static T GetOrDefault<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
        => dictionary.TryGetValue(key, out var value) ? value : default;
}