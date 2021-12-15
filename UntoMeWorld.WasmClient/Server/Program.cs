using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;
using UntoMeWorld.MongoDatabase.Stores;
using UntoMeWorld.WasmClient.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
try
{
    var connectionString = builder.Configuration["MongoConnectionString"];
    
    if (!string.IsNullOrEmpty(connectionString))
        builder.Services.AddSingleton(new MongoDbService(connectionString));
    else
    {
        var mongoDb = builder.Configuration["MongoDatabase"];
        var mongoServer = builder.Configuration["MongoServer"];
        var mongoUsername = builder.Configuration["MongoUsername"];
        var mongoPassword = builder.Configuration["MongoPassword"];
        // Add data store services to the container.
        builder.Services.AddSingleton(new MongoDbService(mongoUsername, mongoPassword, mongoServer, mongoDb ));
    }
    builder.Services.AddSingleton<IChurchesStore, MongoChurchesStore>();
    builder.Services.AddSingleton<IChildrenStore, MongoChildrenStore>();
    builder.Services.AddSingleton<IPastorsStore, MongoPastorsStore>();
// Add services that consume the datastore services and are used by the controllers.
    builder.Services.AddSingleton<IDatabaseService<Church, string>, ChurchesService>();
    builder.Services.AddSingleton<IDatabaseService<Child, string>, ChildrenService>();
    builder.Services.AddSingleton<IDatabaseService<Pastor, string>, PastorsService>();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


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