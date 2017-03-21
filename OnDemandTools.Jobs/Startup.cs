using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Common.DIResolver.Resolvers;
using OnDemandTools.Common.Logzio;
using OnDemandTools.Jobs.Helpers;
using OnDemandTools.Jobs.JobRegistry.CloudAmqpSync;
using OnDemandTools.Jobs.JobRegistry.Deporter;
using OnDemandTools.Jobs.JobRegistry.Mailbox;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using OnDemandTools.Jobs.JobRegistry.TitleSync;
using OnDemandTools.Jobs.Models;
using Serilog;
using StructureMap;
using System;

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
                c.For<IApplicationContext>().Use<JobContext>();
                c.ForSingletonOf<AppSettings>().Use(appSettings);
                c.ForSingletonOf<Serilog.ILogger>().Use(appLogger);
                c.ForSingletonOf<Deporter>();
                c.For<Mailbox>();
                c.For<TitleSync>();                
                c.For<Publisher>();
                c.For<CloudAmqpSync>();
                c.For<DfStatusDeporter>();

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
            // Add serilog and catch any internal errors
            loggerFactory.AddSerilog();

            var appSettings = Configuration.Get<AppSettings>("Application");

            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            if (env.EnvironmentName == "local" || env.EnvironmentName == "development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseErrorLogging();
            app.UseStaticFiles();

            var options = new BackgroundJobServerOptions();
            options.Queues = Enum.GetNames(typeof(HangfireQueue));
            options.WorkerCount = 5;
            GlobalJobFilters.Filters.Add(new HangfireJobExpirationTimeout(appSettings));
            app.UseHangfireServer(options);

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
