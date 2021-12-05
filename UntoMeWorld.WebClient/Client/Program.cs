using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;
using UntoMeWorld.MongoDatabase.Stores;

namespace UntoMeWorld.WebClient.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("UntoMeWorld.WebClient.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("UntoMeWorld.WebClient.ServerAPI"));
            builder.Services.AddBlazoredLocalStorage();
            var service = new MongoDbService("ganduDev", "c4xgvy5lmdLVzihi", "untomeworld.9itjc.mongodb.net", "kids");
            builder.Services.AddSingleton<IChurchesStore>(new MongoChurchesStore(service));
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("api://c3f5c1d4-0938-4f1c-bc87-6de6fdc96493/ServerAPI");
            });

            await builder.Build().RunAsync();
        }
    }
}
