using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Security.Crypto;
using UntoMeWorld.WasmClient.Server.Security.Utils;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.ConfigureAuthorization();

builder.ConfigureMongoDb();

// Add services that consume the datastore services and are used by the controllers.
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddTransient<IChurchesService, ChurchesService>();
builder.Services.AddTransient<IChildrenService, ChildrenService>();
builder.Services.AddTransient<IPastorsService, PastorsService>();
builder.Services.AddTransient<IUserService, AppUsersService>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<IApiAuthorizationService, ApiAuthorizationService>();
builder.Services.AddTransient<ITokensService, TokensService>();
builder.Services.AddTransient<IJwtTokenFactory, JwtTokenFactory>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapFallbackToFile("index.html");
app.Run();