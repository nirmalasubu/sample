
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Serilog;
using OnDemandTools.API.Helpers;
using Microsoft.AspNetCore.Http;
using OnDemandTools.Common.Configuration;
using Serilog.Events;
using OnDemandTools.Common.Logzio;

namespace OnDemandTools.API
{

    /// <summary>
    /// Defining start up configuration
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        Serilog.ILogger AppLogger { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            // Log.Logger = new LoggerConfiguration()
            //           .WriteTo.Logzio("PKzpHRkmGSdahHHIpeprYuKixClLUTRh",
            //           application: "ODT_API",
            //           reporterType: "Monitoring",
            //           environment: "ODT_API_LOCAL")
            //           .CreateLogger();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("Application"));           
            services.AddSingleton<APIBootstrapper>();          
            services.InitializeAutoMapper();
            services.InitializePersistantStorage(); 
                 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            // Add serilog and catch any internal errors
            loggerFactory.AddSerilog();
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

                  
            app.UseMiddleware<SerilogMiddleware>();

            // Specify request pipeline--strictly Nancy middleware
            app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = provider.GetService<APIBootstrapper>()));
        }
    }
    
}
