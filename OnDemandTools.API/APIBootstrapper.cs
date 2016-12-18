using Nancy;
using Nancy.TinyIoc;
using Microsoft.Extensions.Configuration;
using Nancy.Bootstrapper;
using Nancy.Authentication.Stateless;
using NLog;
using OnDemandTools.Utilities.Resolvers;
using System;
using System.Collections.Generic;
using OnDemandTools.Business.Modules.User;
using OOnDemandTools.Utilities.EntityMapping;
using System.Security.Claims;
using OnDemandTools.API.Helpers.MappingRules;

namespace OnDemandTools.API
{



    // Define Nancy bootstrap configurations
    public class APIBootstrapper : DefaultNancyBootstrapper
    {
        public IConfigurationRoot Configuration;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public APIBootstrapper()
        {
            //var builder = new ConfigurationBuilder()
            //                .SetBasePath(RootPathProvider.GetRootPath())
            //                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //                .AddEnvironmentVariables();
            //Configuration = builder.Build();
        }

        public APIBootstrapper(IConfigurationRoot conf)
        {
            Configuration = conf;
        }



        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {            
            container.Register<IConfiguration>(Configuration);
            DependencyResolver.RegisterResolver(new TinyIOCResolver(container)).RegisterImplmentation();
        }



        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {

            //container.Register<ILogic, Logic>();
            base.ApplicationStartup(container, pipelines);

            // Initialize mapping rules
            AutoMapperAPIConfiguration.Configure();
            AutoMapperDomainConfiguration.Configure();
            
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

                Logger.Error(ex);

                return ex;
                // TODO: Add ErrorResponse
                //return ErrorResponse.FromException(ex);
            });

            pipelines.OnError = er;
            StatelessAuthentication.Enable(pipelines, configuration);
        }

       

    }


}
