using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using UntoMeWorld.Application.Services;
using UntoMeWorld.Application.Services.Base;
using UntoMeWorld.Application.Services.Options;
using UntoMeWorld.Infrastructure;
using UntoMeWorld.WasmClient.Server.Common.Swagger;
using UntoMeWorld.WasmClient.Server.Security.Authentication;
using UntoMeWorld.WasmClient.Server.Security.Utils;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddOptions();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    
    builder.Services.ConfigureAuthorization(builder.Configuration);
    builder.Services.RegisterInfrastructure();

// Add services that consume the datastore services and are used by the controllers.
    builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
    builder.Services.AddSingleton<IAuthStateProviderService, AuthStateProviderService>();
    builder.Services.AddTransient<IChurchesService, ChurchesService>();
    builder.Services.AddTransient<IChildrenService, ChildrenService>();
    builder.Services.AddTransient<IUserService, AppUsersService>();
    builder.Services.AddTransient<IRolesService, RolesService>();
    builder.Services.AddTransient<ILogsService, LogsService>();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddServer(new OpenApiServer
        {
            Description = "Local host",
            Url = "https://localhost:5001"
        });
        c.AddServer(new OpenApiServer
        {
            Description = "Production",
            Url = "https://untome.gandu.net"
        });
        c.SupportNonNullableReferenceTypes();
        c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
    });

    builder.Services.Configure<RolesServiceOptions>(builder.Configuration.GetSection("Services")
        .GetSection("RolesService"));
    builder.Services.Configure<UserServiceOptions>(builder.Configuration.GetSection("Services")
        .GetSection("UsersService"));

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

    if (!app.Environment.IsDevelopment())
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        //app.UseHttpsRedirection();
    }
    
    
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    
    app.UseAuthorization();


    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UntoMeWorld API V1"));
    app.MapSwagger();

    app.MapRazorPages();
    app.MapControllers();

    app.MapFallbackToFile("index.html");
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
}