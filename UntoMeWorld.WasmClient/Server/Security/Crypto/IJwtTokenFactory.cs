using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UntoMeWorld.WasmClient.Server.Security.Crypto;

public interface IJwtTokenFactory
{
    public string GenerateToken(IEnumerable<Claim> claims, DateTime expiresOn = default);
    public JwtSecurityToken ReadToken(string token);
    public bool ValidateToken(string token);
}