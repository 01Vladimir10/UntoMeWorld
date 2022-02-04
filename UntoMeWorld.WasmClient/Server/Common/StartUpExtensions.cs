using System.Diagnostics;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;
using UntoMeWorld.MongoDatabase.Stores;

namespace UntoMeWorld.WasmClient.Server.Common;

public static class StartUpExtensions
{
    public static void ConfigureMongoDb(this WebApplicationBuilder builder)
    {
        try
        {
            var connectionString = builder.GetSecureValue("MongoConnectionString");
            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.Services.AddSingleton(new MongoDbService(connectionString));
            }
            else
            {
                var mongoDb = builder.GetSecureValue("MongoDatabase");
                var mongoServer = builder.GetSecureValue("MongoServer");
                var mongoUsername = builder.GetSecureValue("MongoUsername");
                var mongoPassword = builder.GetSecureValue("MongoPassword");
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

    public static string GetSecureValue(this WebApplicationBuilder builder, string key)
        => ServerConstants.Environment == ServerEnvironment.Development ? 
            builder.Configuration[key] ?? Environment.GetEnvironmentVariable(key): 
            Environment.GetEnvironmentVariable(key);
}