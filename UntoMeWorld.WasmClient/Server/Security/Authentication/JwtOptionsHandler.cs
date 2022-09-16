using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using UntoMeWorld.Application.Extensions.Security;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model;

namespace UntoMeWorld.WasmClient.Server.Security.Authentication;

public class JwtOptionsHandler : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly IUserService _service;

    public JwtOptionsHandler(IUserService service)
    {
        _service = service;
    }

    private void Configure(JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                if (!string.IsNullOrEmpty(context.Principal?.ToAppUser().Id))
                    return;
                var userId = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var authProvider = context.Principal?.Claims.FirstOrDefault(c =>
                                       c.Type.Contains("identityProvider", StringComparison.InvariantCultureIgnoreCase))
                                   ?.Value ??
                                   "default";
                if (string.IsNullOrEmpty(userId))
                    return;
                var user = await _service.GetOrCreateUserByThirdPartyAccountInfo(authProvider, userId, () => new AppUser
                {
                    Name = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    Lastname = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                    Email = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Upn)?.Value,
                    Phone = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value,
                });
                var claims = new List<Claim>
                {
                    new(CustomClaims.Roles, string.Join(",", user.Roles ?? new List<string>())),
                    new(CustomClaims.UserId, user.Id),
                    new(CustomClaims.IsDeleted, user.IsDeleted.ToString()),
                    new(CustomClaims.IsDisabled, user.IsDisabled.ToString()),
                };
                context.Principal?.AddIdentity(new ClaimsIdentity(claims));
            }
        };
    }

    public void PostConfigure(string name, JwtBearerOptions options)
    {
        Configure(options);
    }
}