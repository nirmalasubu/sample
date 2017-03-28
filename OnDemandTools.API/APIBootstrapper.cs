using Nancy;
using Nancy.TinyIoc;
using Microsoft.Extensions.Configuration;
using Nancy.Bootstrapper;
using Nancy.Authentication.Stateless;
using System;
using System.Collections.Generic;
using System.IO;
using OnDemandTools.Business.Modules.User;
using System.Security.Claims;
using OnDemandTools.API.v1.Models;
using Serilog;
using OnDemandTools.Common.Logzio;
using OnDemandTools.Common.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Nancy.Conventions;
using System.Linq;
using OnDemandTools.Common.DIResolver;

namespace OnDemandTools.API
{

    // Define Nancy bootstrap configurations
    public class APIBootstrapper : DefaultNancyBootstrapper
    {
        IConfigurationRoot Configuration;
        Serilog.ILogger AppLogger { get; set; }
        IHttpContextAccessor httpContextAccessor;
        AppSettings appSettings;

        public APIBootstrapper(IConfigurationRoot conf, IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> appSettings)
        {
            Configuration = conf;
            this.httpContextAccessor = httpContextAccessor;
            this.appSettings = appSettings.Value;
            ConfigureLogzIOSettings();
        }

        private void ConfigureLogzIOSettings()
        {
            AppLogger = new LoggerConfiguration()
                      .WriteTo.Logzio(appSettings.LogzIO.AuthToken,
                      application: appSettings.LogzIO.Application,
                      reporterType: appSettings.LogzIO.ReporterType,
                      environment: appSettings.LogzIO.Environment)
                      .CreateLogger();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<AppSettings>(appSettings);
            container.Register<IHttpContextAccessor>(httpContextAccessor);
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
                    container.Resolve<IHttpContextAccessor>().HttpContext.User = user;
                    return user;
                }

                return null;
            });

            // Add a default error handler to pipeline
            ErrorPipeline er = new ErrorPipeline();
            er.AddItemToEndOfPipeline((ctx, ex) => LogAndGetErrorResponse(ex, ctx));

            pipelines.OnError = er;
            StatelessAuthentication.Enable(pipelines, configuration);
        }

        private ErrorResponse LogAndGetErrorResponse(Exception ex, NancyContext ctx)
        {
            var errorResponse = ErrorResponse.FromException(ex);
            try
            {
                var messageDictionary = GetErrorProperties(ex, ctx, errorResponse);

                AppLogger.Information(string.Format("{0} {1}", ex.Message, " {Error} "), messageDictionary);
            }
            catch (Exception exception)
            {
                //Logs the actual error message if there is any expception
                AppLogger.Error(ex, ex.Message);

                AppLogger.Information(exception, "Unexpected error occured while logging the error.");
            }

            return errorResponse;
        }

        private Dictionary<string, object> GetErrorProperties(Exception ex, NancyContext ctx, ErrorResponse errorResponse)
        {
            var messageDictionary = new Dictionary<string, object>
            {
                ["stackTrace"] = ex.StackTrace,
                ["requestUrl"] = ctx.Request.Url,
                ["requestHostAddress"] = ctx.Request.UserHostAddress
            };

            try
            {
                if (ctx.Request.Body.Length > 0)
                {
                    using (var sr = new StreamReader(ctx.Request.Body))
                    {
                        sr.BaseStream.Seek(0, SeekOrigin.Begin);

                        var requestBody = sr.ReadToEnd();

                        messageDictionary["requestBody"] = requestBody;

                    }
                }
            }
            catch (Exception e)
            {
                AppLogger.Error(e, "Unexpected error occured while reading request body.");
            }


            if (ex.InnerException != null)
            {
                messageDictionary["innerException"] = string.Format("{0} - {1}", ex.InnerException.Message,
                    ex.InnerException.StackTrace ?? string.Empty);
            }

            try
            {
                var apiKey = ctx.Request.Headers.Authorization;
                if (apiKey.Length > 7)
                    messageDictionary["apiKey"] = apiKey.Substring(apiKey.Length - 7);
            }
            catch (Exception e)
            {
                AppLogger.Error(e, "Unexpected error occured while reading api key.");
            }

            messageDictionary["statusCode"] = errorResponse.StatusCode;

            return messageDictionary;
        }


        /// <summary>
        /// Configures the conventions in NancyFx.
        /// </summary>
        /// <param name="nancyConventions">The nancy conventions.</param>
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            this.Conventions.AcceptHeaderCoercionConventions.Add((acceptHeaders, ctx) =>
            {
                // Provide default accept header - 'application/json' if no valid ['application/json' , 'application/xml'] accept headers are provided.
                // This is only applicable for the API route. Note, this is a temporary solution. A better approach would be
                // to use dedicated API handler. In the future if more customization is requested then the need for a
                // dedicated API handler is warranted
                if (ctx.Request.Path.Contains("/v1/"))
                {
                    var current = acceptHeaders as Tuple<string, decimal>[] ?? acceptHeaders.ToArray();
                    var validHeaders = acceptHeaders.Where(h => (string.Equals(h.Item1, "application/json", StringComparison.OrdinalIgnoreCase) && h.Item2 == (decimal)1) ||
                                          (string.Equals(h.Item1, "application/xml", StringComparison.OrdinalIgnoreCase)) && h.Item2 == (decimal)1).ToList();

                    if (validHeaders.Count() <= 0)
                    {
                        return new[] { Tuple.Create("application/json", (decimal)1) };
                    }
                }

                return acceptHeaders;
            });
        }

    }


}
