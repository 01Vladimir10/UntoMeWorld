using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.WasmClient.Client;
using UntoMeWorld.WasmClient.Client.Components.Interop;
using UntoMeWorld.WasmClient.Client.Data.Stores;
using UntoMeWorld.WasmClient.Client.Services;
using UntoMeWorld.WasmClient.Client.Services.Base;
using UntoMeWorld.WasmClient.Client.Services.Security;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services
    .AddHttpClient("UntoMeWorld.WasmClient.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddSingleton<ToastService>();
builder.Services.AddTransient<IChurchesStore, RemoteChurchesStore>();
builder.Services.AddTransient<IChildrenStore, RemoteChildrenStore>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<IAuthorizationProviderService, ApiAuthorizationService>();
builder.Services.AddTransient<IChurchesService, ChurchesService>();
builder.Services.AddTransient<IPastorService, PastorsService>();
builder.Services.AddTransient<IChildrenService, ChildrenService>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("UntoMeWorld.WasmClient.ServerAPI"));



builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://c3f5c1d4-0938-4f1c-bc87-6de6fdc96493/ServerAPI");
});

await builder.Build().RunAsync();