using AutoMapper;
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
using QuoteMyGoods.Models;
using QuoteMyGoods.Services;
using System;
//using AutoMapper;

namespace QuoteMyGoods
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

            /*
            services.AddSingleton<IDistributedCache>(
                serviceProvider =>
                    new Microsoft.Extensions.Caching.Redis.RedisCache(new RedisCacheOptions
                    {
                        Configuration = "qmgrediscache.redis.cache.windows.net:6380,password=beSaRecMqNGWrES1pVKvQPzpNq6GJs1Omlmolc4KeB0=,ssl=True,abortConnect=False",
                        InstanceName = "qmgrediscache"
                    }));
                    */

            //services.AddCaching();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = "QMG";
            });

            services.AddIdentity<QMGUser, IdentityRole>(config =>
             {
                 config.User.RequireUniqueEmail = true;
                 config.Password.RequiredLength = 8;
                 config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                 config.Cookies.ApplicationCookie.AccessDeniedPath = "/Auth/Unauthorized";
                 config.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
             })
            .AddEntityFrameworkStores<QMGContext>();

            services.AddLogging();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<QMGContext>();

            services.AddSingleton<ITableService,TableService>();
            services.AddTransient<QMGContextSeedData>();
            services.AddScoped<IQMGRepository, QMGRepository>();
            services.AddScoped<IMailService,MailService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<IBlobbingService, BlobbingService>();
            services.AddSingleton<IRedisService, RedisService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, QMGContextSeedData seeder)
        {
            //change to debug in production env
            loggerFactory.AddDebug(LogLevel.Information);
            app.UseDeveloperExceptionPage();

            app.UseSession();

            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Product, Product>().ReverseMap();
            });

            app.UseMvc(config =>
            {
            config.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "App", action = "Index" }
                );
            });

            await seeder.EnsureSeedDataAsync();
        }  
    }
}
