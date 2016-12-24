
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Serilog;
using System.IO;
using Serilog.Formatting;
using Serilog.Events;
using OnDemandTools.Common.Logzio;
using AutoMapper;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyModel;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using OnDemandTools.API.Helpers;

namespace OnDemandTools.API
{

    /// <summary>
    /// Defining start up configuration
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Serilog.ILogger AppLogger{ get; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
           
            //TODO - create a section for this in appsettings.json
            AppLogger = new LoggerConfiguration()                                             
                       .WriteTo.Logzio("PKzpHRkmGSdahHHIpeprYuKixClLUTRh", application:"ODT", reporterType:"DDDD", environment:"DEV")
                       .CreateLogger();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.InitializeAutoMapper();
       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            // Add serilog and catch any internal errors
            loggerFactory.AddSerilog();
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));


            // Specify request pipeline--strictly Nancy middleware
            app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new APIBootstrapper(Configuration, AppLogger)));
        }
    }
    
}
