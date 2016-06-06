using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using QuoteMyGoods.Data;
using QuoteMyGoods.Data.Products;
using System;

namespace QuoteMyGoods.Web
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Business.ModelMappings.Configure();
            ModelMappings.Configure();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                   .RequireAuthenticatedUser()
                                   .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddDbContext<ProductContext>(options =>
                options.UseSqlServer(Configuration["Data:QMGContextConnection"]));

            /*
            services.AddSingleton<IDistributedCache>(
                serviceProvider =>
                    new Microsoft.Extensions.Caching.Redis.RedisCache(new RedisCacheOptions
                    {
                        Configuration = "qmgrediscache.redis.cache.windows.net:6380,password=beSaRecMqNGWrES1pVKvQPzpNq6GJs1Omlmolc4KeB0=,ssl=True,abortConnect=False",
                        InstanceName = "qmgrediscache"
                    }));
                    */

            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = "QMG";
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
             {
                 config.User.RequireUniqueEmail = true;
                 config.Password.RequiredLength = 8;
                 config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                 config.Cookies.ApplicationCookie.AccessDeniedPath = "/Auth/Unauthorized";
                 config.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
             })
            .AddEntityFrameworkStores<ProductContext>();

            services.AddLogging();

            services.AddTransient<QMGContextSeedData>();
            DependencyInjection.Configuration(services);
        }

        public async void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, QMGContextSeedData seeder)
        {
            loggerFactory.AddDebug(LogLevel.Information);
            app.UseDeveloperExceptionPage();

            app.UseSession();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                  template: "{area:exists}/{controller=App}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=App}/{action=Index}");
            });

            //await seeder.EnsureSeedDataAsync();
        }  
    }
}
