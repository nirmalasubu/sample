using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Common.DIResolver.Resolvers;
using OnDemandTools.Common.Logzio;
using OnDemandTools.Web.Helpers;
using Serilog;
using StructureMap;
using System;

namespace OnDemandTools.Web
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
            });

            // Initialize automapper
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
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));


            //// Display user friendly errors when not in development
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}

            //app.UseErrorLogging();
            app.UseStaticFiles();

            // Default mvc format
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default_route",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
        }
    }
}
