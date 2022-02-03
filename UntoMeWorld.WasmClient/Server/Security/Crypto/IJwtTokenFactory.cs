using System.IdentityModel.Tokens.Jwt;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Security.Crypto;

public interface IJwtTokenFactory
{
    public Task<string> GenerateToken(AppUser user, string description = "", DateTime expiresOn = default);
    public JwtSecurityToken ReadToken(string token);

    public bool ValidateToken(string token);
}