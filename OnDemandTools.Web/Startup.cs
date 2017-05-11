using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Http;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Web.Helpers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using StructureMap;
using OnDemandTools.Common.Logzio;
using OnDemandTools.Common.DIResolver.Resolvers;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Business.Modules.User;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using OnDemandTools.Web.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using OnDemandTools.Web.Utilities.Redis;

namespace OnDemandTools.Web
{
    public class Startup
    {
        StructureMap.Container container;
        public static IConnectionManager ConnectionManager;

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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            var appSettings = Configuration.Get<AppSettings>("Application");


            // Add framework services.
            services.AddMvc();    
            //services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache

            // Using Redis as Memory cache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = appSettings.Redis.Url;
                options.InstanceName = appSettings.Redis.InstanceName;
                options.ResolveDns();
            });
            services.AddSession();
            services.AddSignalR();

            // Predefine set of policies        
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN", policy => {
                    policy.RequireClaim("admin");
                });

                 options.AddPolicy("READ", policy => {
                    policy.RequireClaim("read");
                });

                options.AddPolicy("WRITE", policy => {
                    policy.RequireClaim("write");
                });

                options.AddPolicy("GET", policy => {
                    policy.RequireClaim("get");
                });

                options.AddPolicy("POST", policy => {
                    policy.RequireClaim("post");
                });

                 options.AddPolicy("DELETE", policy => {
                    policy.RequireClaim("delete");
                });
            });

            // Add Authentication services.
            services.AddAuthentication(sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            // Initialize container            
            container = new Container();
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

            services.InitializeAutoMapper();


            // Resolve container 
            DependencyResolver.RegisterResolver(new StructureMapIOCContainer(container)).RegisterImplmentation();
            container.Populate(services);

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider, IDeliveryQueueData deliveryQueueData, IDistributedCache cache, Serilog.ILogger logger)
        {

            // Add serilog and catch any internal errors
            loggerFactory.AddSerilog();
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            // Set logging based on enviroments
            if (env.IsEnvironment("local") || env.IsEnvironment("development"))
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");                 
            }

            // server static files
            app.UseStaticFiles();

            app.UseSignalR();

            // Configure the OWIN pipeline to use cookie auth.
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
                SlidingExpiration = true
            });

            // Configure the OWIN pipeline to use OpenID Connect auth.
            AppSettings settings = Configuration.Get<AppSettings>("Application");
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                ClientId = settings.AzureAd.ClientId,
                Authority = String.Format(settings.AzureAd.AadInstance, settings.AzureAd.Tenant),
                ResponseType = OpenIdConnectResponseType.IdToken,
                PostLogoutRedirectUri = settings.AzureAd.PostLogoutRedirectUri,
                Events = new OpenIdConnectEvents
                {
                    OnAuthenticationFailed = cx =>  AuthenticationFailed(cx),
                    OnRemoteFailure = cx=> RemoteFailure(cx),
                    OnTokenValidated = cx => TokenValidated(cx)
                },             
                CallbackPath = "/signin-oidc"
            });

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
          
            ConnectionManager = provider.GetService<IConnectionManager>();
            AutomaticViewRefresher automaticViewRefresher = new AutomaticViewRefresher(deliveryQueueData,logger);
            automaticViewRefresher.Start(10);
        }
       

        // Once the user is successfully authenticated, add additional
        // claims that are specific to ODT and user context
        private Task TokenValidated(TokenValidatedContext n)
        {  
            n.Ticket.Principal.AddIdentity(container.GetInstance<IUserHelper>().GetByUserName( n.Ticket.Principal.Identity.Name).Identities.FirstOrDefault());            
            n.Properties.RedirectUri = "/Account/Gatekeeper";
            return Task.FromResult(0);
        }

        // Handle sign-in errors differently than generic errors.
        private Task RemoteFailure(FailureContext context)
        {
            context.HandleResponse();
           context.Response.Redirect("/");
            //context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
            return Task.FromResult(0);
        }

        // Handle sign-in errors differently than generic errors.
        private Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/");
            //context.Response.Redirect("/Home/Error?message=" + context.Response.ToString());
            return Task.FromResult(0);
        }
    }
}
