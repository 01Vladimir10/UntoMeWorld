using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using UntoMeWorld.WasmClient.Server.Security.Authentication;
using UntoMeWorld.WasmClient.Server.Security.Authorization;

namespace UntoMeWorld.WasmClient.Server.Security.Utils;

public static class SecurityExtensions
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.AddRequirements(new ApiAuthorizationRequirement());
            options.DefaultPolicy = policyBuilder.Build();
            
            options.AddPolicy("UserAuthenticationOnly", builder =>
            {
                builder.AddRequirements(new ApiAuthorizationRequirement
                {
                    AllowTokenAuthentication = false,
                    AllowUsersAuthentication = true
                });
            });
        });
        services.AddSingleton<IAuthorizationHandler, ApiAuthorizationHandler>();
        services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtOptionsHandler>();
    }
}