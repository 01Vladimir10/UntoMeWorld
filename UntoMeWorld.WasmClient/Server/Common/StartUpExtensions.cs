using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;
using UntoMeWorld.MongoDatabase.Stores;
using UntoMeWorld.WasmClient.Server.Security.Authorization;

namespace UntoMeWorld.WasmClient.Server.Common;

public static class StartUpExtensions
{
    public static void ConfigureMongoDb(this WebApplicationBuilder builder)
    {
        try
        {
            var connectionString = builder.Configuration["MongoConnectionString"];
            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.Services.AddSingleton(new MongoDbService(connectionString));
            }
            else
            {
                var mongoDb = builder.Configuration["MongoDatabase"];
                var mongoServer = builder.Configuration["MongoServer"];
                var mongoUsername = builder.Configuration["MongoUsername"];
                var mongoPassword = builder.Configuration["MongoPassword"];
                // Add data store services to the container.
                builder.Services.AddSingleton(new MongoDbService(mongoUsername, mongoPassword, mongoServer, mongoDb));
            }

            builder.Services.AddTransient<IChurchesStore, MongoChurchesStore>();
            builder.Services.AddTransient<IChildrenStore, MongoChildrenStore>();
            builder.Services.AddTransient<IPastorsStore, MongoPastorsStore>();
            
            builder.Services.AddTransient<ITokenStore, MongoTokensStore>();
            builder.Services.AddTransient<IRoleStore, MongoRolesStore>();
            builder.Services.AddTransient<IUserStore, MongoUsersStore>();
            Debug.WriteLine("Mongodb set up was successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void AddJwtAuthenticationListener(this IServiceCollection collection, Func<TokenValidatedContext, Task> listener)
    {
        collection.Configure<JwtBearerOptions>(options =>
        {
            options.Events.OnTokenValidated = listener;
        });
    }

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
    }
}