using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.ConfigureAuthorization();

builder.Services.AddSingleton<ICacheService, InMemoryCache>();
builder.ConfigureMongoDb();

// Add services that consume the datastore services and are used by the controllers.
builder.Services.AddSingleton<IChurchesService, ChurchesService>();
builder.Services.AddSingleton<IChildrenService, ChildrenService>();
builder.Services.AddSingleton<IPastorsService, PastorsService>();
builder.Services.AddSingleton<IUserService, AppUsersService>();
builder.Services.AddSingleton<IRolesService, RolesService>();
builder.Services.AddSingleton<IApiAuthorizationService, ApiAuthorizationService>();
builder.Services.AddSingleton<ITokensService, TokensService>();

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