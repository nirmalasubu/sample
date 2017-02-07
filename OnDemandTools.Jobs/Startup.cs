using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnDemandTools.Common.Configuration;
using StructureMap;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Common.DIResolver.Resolvers;
using Serilog;
using OnDemandTools.Common.Logzio;
using Hangfire;
using Hangfire.Mongo;
using OnDemandTools.Jobs.JobRegistry.Deporter;
using OnDemandTools.Jobs.JobRegistry.TitleSync;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using OnDemandTools.Jobs.Helpers;

namespace OnDemandTools.Jobs
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Retrieve configurations
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            var appSettings = Configuration.Get<AppSettings>("Application");

            // Add framework services.
            services.AddMvc();
            services.AddHangfire(x => x.UseMongoStorage(appSettings.MongoDB.HangfireConnectionString + appSettings.MongoDB.HangfireConnectionOptions,
                appSettings.MongoDB.HangFireDatabaseName));

            // Initialize container
            var container = new Container();
            container.Configure(c =>
            {
                Serilog.ILogger appLogger = new LoggerConfiguration()
                     .WriteTo.Logzio(appSettings.LogzIO.AuthToken,
                     application: appSettings.LogzIO.Application,
                     reporterType: appSettings.LogzIO.ReporterType,
                     environment: appSettings.LogzIO.Environment)
                     .CreateLogger();

                c.ForSingletonOf<AppSettings>().Use(appSettings);
                c.ForSingletonOf<Serilog.ILogger>().Use(appLogger);
                c.ForSingletonOf<Deporter>();
                c.ForSingletonOf<TitleSync>();
                c.ForSingletonOf<Publisher>();
            });

            services.InitializeAutoMapper();

            // Resolve container 
            DependencyResolver.RegisterResolver(new StructureMapIOCContainer(container)).RegisterImplmentation();
            container.Populate(services);

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                // Add serilog and catch any internal errors
                loggerFactory.AddSerilog();
                Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));


                app.UseStaticFiles();
                app.UseHangfireServer();

                //TODO - temporary solution to allow all users. Will need to come up with an
                // authorization mechanism
                app.UseHangfireDashboard("/dashboard", new DashboardOptions
                {
                    Authorization = new[] { new CustomAuthorizationFilter() }
                });

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=home}/{action=Index}/{id?}");
                });
            }
            catch (Exception e)
            {               
                throw e;
            }
        }
    }

    //TODO - temporary solution to allow all users. Will need to come up with an
    // authorization mechanism
    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
