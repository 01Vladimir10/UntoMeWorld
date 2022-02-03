﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Constants;

namespace UntoMeWorld.WasmClient.Server.Security.Utils;

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

    public static AppUser ToAppUser(this ClaimsPrincipal claimsArr)
        => claimsArr.Claims.ToAppUser();
    public static string GetTokenId(this JwtSecurityToken token)
        => token.Claims.FirstOrDefault(c => c.Type == CustomClaims.TokenId)?.Value ?? string.Empty;
    public static string GetUserId(this JwtSecurityToken token)
        => token.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId)?.Value ?? string.Empty;
    public static IEnumerable<string> GetRoles(this JwtSecurityToken token)
        => token.Claims.FirstOrDefault(c => c.Type == CustomClaims.Roles)?.Value?.Split(",") ?? Array.Empty<string>();

    private static T GetOrDefault<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
        => dictionary.TryGetValue(key, out var value) ? value : default;
}