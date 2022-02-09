using System.Diagnostics;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;
using UntoMeWorld.MongoDatabase.Stores;

namespace UntoMeWorld.WasmClient.Server.Common;

public static class StartUpExtensions
{
    public static void UseMongoDb(this IServiceCollection services)
    {
        try
        {
            var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariables.DatabaseConnectionString);
            var database = Environment.GetEnvironmentVariable(EnvironmentVariables.DatabaseName);
            services.AddSingleton(new MongoDbService(connectionString, database));
            services.AddTransient<IChurchesStore, MongoChurchesStore>();
            services.AddTransient<IChildrenStore, MongoChildrenStore>();
            services.AddTransient<IPastorsStore, MongoPastorsStore>();
            services.AddTransient<ITokenStore, MongoTokensStore>();
            services.AddTransient<IRoleStore, MongoRolesStore>();
            services.AddTransient<IUserStore, MongoUsersStore>();
            Debug.WriteLine("Mongodb set up was successful!");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }
}