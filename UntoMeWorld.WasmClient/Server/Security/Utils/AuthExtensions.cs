using System.IdentityModel.Tokens.Jwt;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Constants;

namespace UntoMeWorld.WasmClient.Server.Security.Utils;

public static class AuthExtensions
{
    public static Token ToToken(this JwtSecurityToken token)
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


}