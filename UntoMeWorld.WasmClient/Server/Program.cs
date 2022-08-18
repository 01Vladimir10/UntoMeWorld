using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using UntoMeWorld.WasmClient.Server.Common;
using UntoMeWorld.WasmClient.Server.Common.Swagger;
using UntoMeWorld.WasmClient.Server.Security.Utils;
using UntoMeWorld.WasmClient.Server.Services;
using UntoMeWorld.WasmClient.Server.Services.Base;
using UntoMeWorld.WasmClient.Server.Services.Options;

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddOptions();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    builder.Services.ConfigureAuthorization(builder.Configuration);

    builder.Services.UseMongoDb();

// Add services that consume the datastore services and are used by the controllers.
    builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
    builder.Services.AddTransient<IChurchesService, ChurchesService>();
    builder.Services.AddTransient<IChildrenService, ChildrenService>();
    builder.Services.AddTransient<IPastorsService, PastorsService>();
    builder.Services.AddTransient<IUserService, AppUsersService>();
    builder.Services.AddTransient<IRolesService, RolesService>();
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddServer(new OpenApiServer
        {
            Description = "Local host",
            Url = "https://localhost:5001"
        });
        c.SupportNonNullableReferenceTypes();
        c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
    });

    builder.Services.Configure<RolesServiceOptions>(builder.Configuration.GetSection("Services").GetSection("RolesService"));
    builder.Services.Configure<UserServiceOptions>(builder.Configuration.GetSection("Services").GetSection("UsersService"));

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
    app.Urls.Add("https://*:5001");
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