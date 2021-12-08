using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UntoMeWorld.Domain.Stores;
using UntoMeWorld.MongoDatabase.Services;
using UntoMeWorld.MongoDatabase.Stores;
using UntoMeWorld.WebClient.Server.Services;

namespace UntoMeWorld.WebClient.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mongoDb = Configuration["MongoDatabase"];
            var mongoServer = Configuration["MongoServer"];
            var mongoUsername = Configuration["MongoUsername"];
            var mongoPassword = Configuration["MongoPassword"];
            services.AddSingleton(new MongoDbService(mongoUsername, mongoPassword, mongoServer, mongoDb ));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            // Add stores.
            services.AddSingleton<IChurchesStore, MongoChurchesStore>();
            services.AddSingleton<IChildrenStore, MongoChildrenStore>();
            // Add services
            services.AddSingleton<ChurchesService>();
            services.AddSingleton<ChildrenService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}