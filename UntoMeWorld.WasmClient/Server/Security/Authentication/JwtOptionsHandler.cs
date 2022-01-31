using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.WasmClient.Server.Security.Constants;
using UntoMeWorld.WasmClient.Server.Services.Base;

namespace UntoMeWorld.WasmClient.Server.Security.Authentication;

public class JwtOptionsHandler : IConfigureOptions<JwtBearerOptions>
{
    private readonly IUserService _service;

    public JwtOptionsHandler(IUserService service)
    {
        _service = service;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.Events.OnTokenValidated = async context =>
        {
            var userId = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return;
            var user = await _service.GetOrCreateUserByThirdPartyAccountInfo("AzureAd", userId, () => new AppUser
            {
                Name = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Lastname = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                Email = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Phone = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value,
            });
            var claims = new List<Claim>
            {
                new(CustomClaims.Roles, string.Join(",", user.RoleIds)),
                new(CustomClaims.UserId, user.Id),
                new(CustomClaims.IsDeleted, user.IsDeleted.ToString()),
                new(CustomClaims.IsDisabled, user.IsDisabled.ToString()),
            };
            context.Principal.AddIdentity(new ClaimsIdentity(claims));
        };
    }
}