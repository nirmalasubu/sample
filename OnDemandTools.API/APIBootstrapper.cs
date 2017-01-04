using Nancy;
using Nancy.TinyIoc;
using Microsoft.Extensions.Configuration;
using Nancy.Bootstrapper;
using Nancy.Authentication.Stateless;
using OnDemandTools.Utilities.Resolvers;
using System;
using System.Collections.Generic;
using OnDemandTools.Business.Modules.User;
using System.Security.Claims;
using OnDemandTools.API.v1.Models;
using Serilog;
using OnDemandTools.Common.Logzio;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.API
{

    // Define Nancy bootstrap configurations
    public class APIBootstrapper : DefaultNancyBootstrapper
    {
        public IConfigurationRoot Configuration;        
        public Serilog.ILogger AppLogger { get; set; }
       

        public APIBootstrapper(IConfigurationRoot conf)
        {
            Configuration = conf;
            ConfigureLogzIOSettings(conf);
        }

        private void ConfigureLogzIOSettings(IConfigurationRoot conf)
        {
            LogzIOConfiguration logzConf = new LogzIOConfiguration();
            conf.GetSection("logzIO").Bind(logzConf);
            AppLogger = new LoggerConfiguration()
                      .WriteTo.Logzio(logzConf.AuthToken,
                      application: logzConf.Application,
                      reporterType: logzConf.ReporterType,
                      environment: logzConf.Environment)
                      .CreateLogger();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {           
            container.Register<Serilog.ILogger>(AppLogger);
            container.Register<IConfiguration>(Configuration);
            DependencyResolver.RegisterResolver(new TinyIOCResolver(container)).RegisterImplmentation();
        }



        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
        }



        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {

            // Identify request holder through API key
            var configuration = new StatelessAuthenticationConfiguration(ctx =>
            {
                Guid apiKey;

                // If API key found, retrieve user information associated with the 
                // API key. If user found, add user information to request
                if (Guid.TryParse(ctx.Request.Headers.Authorization, out apiKey))
                {
                    var query = container.Resolve<IUserHelper>();
                    ClaimsPrincipal user = query.GetBy(apiKey);

                    if (user.Identity.Name == string.Empty)
                    {
                        return null;
                    }

                    // Add user information 
                    ctx.Items.Add(new KeyValuePair<string, object>("user", user.Identity));
                    return user;
                }
                
                return null;
            });



            // Add a default error handler to pipeline
            ErrorPipeline er = new ErrorPipeline();
            er.AddItemToEndOfPipeline((ctx, ex) =>
            {
                AppLogger.Error(ex, ex.Message);
                return ErrorResponse.FromException(ex);
                       
            });

            pipelines.OnError = er;
            StatelessAuthentication.Enable(pipelines, configuration);
        }
       
    }


}
