using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.Application.Extensions.Security;

public static class AuthExtensions
{
    public static Token? ToToken(this JwtSecurityToken? token)
        => token == null ? null : new Token
        {
            Id = token.Claims.FirstOrDefault(c => c.Type == CustomClaims.TokenId)?.Value,
            CreatedOn = token.IssuedAt,
            Roles = token.Claims.FirstOrDefault(c => c.Type == CustomClaims.Roles)?.Value.Split(",").ToList() ?? new List<string>(),
        };
    public static string GetTokenId(this JwtSecurityToken token)
        => token.Claims.FirstOrDefault(c => c.Type == CustomClaims.TokenId)?.Value ?? string.Empty;
    public static string GetUserId(this JwtSecurityToken token)
        => token.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId)?.Value ?? string.Empty;
    public static IEnumerable<string> GetRoles(this JwtSecurityToken token)
        => token.Claims.FirstOrDefault(c => c.Type == CustomClaims.Roles)?.Value?.Split(",") ?? Array.Empty<string>();
    public static AppUser ToAppUser(this IEnumerable<Claim> claimsArr)
    {
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

    public static IEnumerable<Claim?> ToClaims(this Token? token)
        => token == null ? Array.Empty<Claim>() :
            new Claim[]
            {
                new(CustomClaims.TokenId, token.Id),
                new(CustomClaims.Roles, string.Join(",", token.Roles))
            };

    public static AppUser ToAppUser(this ClaimsPrincipal claimsArr)
        => claimsArr.Claims.ToAppUser();

}