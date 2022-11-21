using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using UntoMeWorld.Application.Common;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Infrastructure.Services;
using UntoMeWorld.Infrastructure.Stores;

namespace UntoMeWorld.Infrastructure;

public static class StartUpExtensions
{
    public static void RegisterInfrastructure(this IServiceCollection services)
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
            services.AddTransient<ILabelReportsStore, MongoLabelReportStore>();
            services.AddTransient<IActionLogsStore, MongoActionLogsStore>();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }
}