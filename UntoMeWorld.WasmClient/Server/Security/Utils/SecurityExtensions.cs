using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Services;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Services.Crypto;
using UntoMeWorld.Application.Services.Options;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.WasmClient.Server.Security.Authentication;
using UntoMeWorld.WasmClient.Server.Security.Authorization;
using UntoMeWorld.WasmClient.Server.Services.Security;

namespace UntoMeWorld.WasmClient.Server.Security.Utils;

public static class SecurityExtensions
{
    public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
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
        services.AddTransient<IApiAuthorizationService, ApiAuthorizationService>();
        services.AddTransient<ITokensService, TokensService>();
        services.AddTransient<IJwtTokenFactory, JwtTokenFactory>();

        services.Configure<JwtTokenFactoryOptions>(options =>
        {
            configuration.Bind("Jwt", options);
            options.Secret = Environment.GetEnvironmentVariable(EnvironmentVariables.JwtSecret) ?? Guid.NewGuid().ToString();
        });
        var servicesConfiguration = configuration.GetSection("Services");
        services.Configure<AuthorizationServiceOptions>(servicesConfiguration.GetSection("AuthorizationService"));
        services.Configure<TokenServiceOptions>(servicesConfiguration.GetSection("TokensService"));
    }
}