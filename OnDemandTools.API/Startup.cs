
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using NLog.Extensions.Logging;

namespace OnDemandTools.API
{

    /// <summary>
    /// Defining start up configuration
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }        


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // All dependency injection will be done in APIBootstrapper
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add Nlog to pipeline. Configuration will be read from nlog.config
            loggerFactory.AddNLog();
             
             
            // Specify request pipeline--strictly Nancy middleware
            app.UseOwin(x => x.UseNancy(opt=>opt.Bootstrapper = new APIBootstrapper(Configuration)));
        }

    }
}
