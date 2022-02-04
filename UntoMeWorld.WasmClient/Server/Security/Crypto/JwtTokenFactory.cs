using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Constants;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Security.Crypto;

public class JwtTokenFactory : IJwtTokenFactory
{
    private readonly IConfiguration _configuration;

    private string Issuer => _configuration.GetSection("Jwt")["Issuer"];
    private string Audience => _configuration.GetSection("Jwt")["Audience"];
    private string Secret => _configuration.GetSection("Jwt")["Secret"];

    public JwtTokenFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // TODO: relay creation of the token to the controller/service.
    public string GenerateToken(IEnumerable<Claim> claims, DateTime expiresOn = default)
    {
        var secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        var handler = new JwtSecurityTokenHandler();

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresOn == default ? DateTime.Now.AddMonths(1) : expiresOn,
            Issuer = Issuer,
            Audience = Audience,
            SigningCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature),
        };
        var jwtToken = handler.CreateToken(descriptor);
        return handler.WriteToken(jwtToken);
    }


    public JwtSecurityToken ReadToken(string token)
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