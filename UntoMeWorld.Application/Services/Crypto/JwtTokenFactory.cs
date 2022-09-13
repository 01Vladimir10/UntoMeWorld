using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UntoMeWorld.Application.Services.Options;

namespace UntoMeWorld.Application.Services.Crypto;

public class JwtTokenFactory : IJwtTokenFactory
{private string Issuer => _options.Issuer;
    private string Audience => _options.Audience;
    private string Secret => _options.Secret;

    private readonly JwtTokenFactoryOptions _options;

    public JwtTokenFactory(IOptions<JwtTokenFactoryOptions> options)
    {
        _options = options.Value;
    }

    // TODO: relay creation of the token to the controller/service.
    public string GenerateToken(IEnumerable<Claim?> claims, DateTime expiresOn = default)
    {
        var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        var handler = new JwtSecurityTokenHandler();

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresOn == default ? DateTime.UtcNow.AddMinutes(_options.DefaultTokenDurationInMinutes) : expiresOn,
            Issuer = Issuer,
            Audience = Audience,
            SigningCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature),
        };
        var jwtToken = handler.CreateToken(descriptor);
        return handler.WriteToken(jwtToken);
    }


    public JwtSecurityToken? ReadToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return securityToken;
    }

    public bool ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        var handler = new JwtSecurityTokenHandler();
        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = Issuer,
                ValidAudience = Issuer,
                IssuerSigningKey = key
            }, out _);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}