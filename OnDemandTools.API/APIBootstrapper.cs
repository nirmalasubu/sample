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
using OnDemandTools.Business.Modules.User.Model;
using AutoMapper;
using OOnDemandTools.Utilities.EntityMapping;
using System.Security.Claims;

namespace OnDemandTools.API
{



    // Define Nancy bootstrap configurations
    public class APIBootstrapper : DefaultNancyBootstrapper
    {
        public IConfigurationRoot Configuration;
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public APIBootstrapper()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(RootPathProvider.GetRootPath())
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables();
            Configuration = builder.Build();
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

            // Define mapping rules
            var profiles = new List<Profile>();           
            MappingBootstrapper.Map(profiles);
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
                    ctx.Items.Add(new KeyValuePair<string, object>("user", user));
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
