
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



    //    //var assembliesToScan = DependencyContext.Default

    //    var k = GetReferencingAssemblies("OnDemandTools")
    //            .SelectMany(assembly => assembly.ExportedTypes)
    //            .ToList();

    //    //foreach (var item in k)
    //    //{
    //    //    var kkkr = item.GetTypeInfo().ToString();
    //    //    //Console.WriteLine(item.DeclaringType.ToString());
    //    //    //Console.WriteLine(item.GetElementType().ToString());
    //    //    var kkk = item;
    //    //}


    //    var profiles =k
    //.Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
    //.Where(t => !t.GetTypeInfo().IsAbstract);

    //    Mapper.Initialize(cfg =>
    //    {
    //        foreach (var profile in profiles)
    //        {
    //            cfg.AddProfile(profile);
    //        }
    //    });

    //var k = DependencyContext.Default.RuntimeLibraries
    //        // Only load assemblies that reference AutoMapper
    //        .Where(lib =>

    //            lib.Dependencies.Any(d => d.Name.Equals("OnDemandTools.MappingRules", StringComparison.OrdinalIgnoreCase)));


    //services.useAutoMapper();
    //var allTypes = assembliesToScan.SelectMany(a => a.ExportedTypes).ToArray();

    //Mapper.Initialize(cfg =>
    //{
    //    cfg.AddProfile(k[24]);
    //});

    //var dd = "dd";
    //var allTypes = assembliesToScan.SelectMany(a => a.Exp).ToArray();

    //var profiles =
    //allTypes
    //    .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
    //    .Where(t => !t.GetTypeInfo().IsAbstract);

    //Mapper.Initialize(cfg =>
    //{
    //    foreach (var profile in profiles)
    //    {
    //        cfg.AddProfile(profile);
    //    }
    //});


    //private static void AddAutoMapper(IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
    //{
    //    assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

    //    var allTypes = assembliesToScan.SelectMany(a => a.ExportedTypes).ToArray();

    //    var profiles =
    //    allTypes
    //        .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
    //        .Where(t => !t.GetTypeInfo().IsAbstract);

    //    Mapper.Initialize(cfg =>
    //    {
    //        foreach (var profile in profiles)
    //        {
    //            cfg.AddProfile(profile);
    //        }
    //    });
    //}

    //public static void AddAutoMapper(this IServiceCollection services, DependencyContext dependencyContext)
    //{
    //    services.AddAutoMapper(dependencyContext.RuntimeLibraries
    //        .SelectMany(lib => lib.GetDefaultAssemblyNames(dependencyContext).Select(Assembly.Load)));
    //}

    //public static void AddAutoMapper(this IServiceCollection services)
    //{
    //    services.AddAutoMapper(DependencyContext.Default);
    //}
    //private bool IsCandidateLibrary(RuntimeLibrary library, String assemblyName)
    //{
    //    return library.Name == (assemblyName)
    //        || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
    //}

    //public IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
    //{
    //    var assemblies = new List<Assembly>();
    //    var dependencies = DependencyContext.Default.RuntimeLibraries;
    //    foreach (var library in dependencies)
    //    {
    //        if (IsCandidateLibrary(library, assemblyName))
    //        {
    //            var assembly = Assembly.Load(new AssemblyName(library.Name));
    //            assemblies.Add(assembly);
    //        }
    //    }
    //    return assemblies;
    //}
    //// ReSharper disable once CheckNamespace
    //namespace Microsoft.Extensions.DependencyInjection
    //{
    //    /// <summary>
    //    /// The auto mapper extensions.
    //    /// </summary>
    //    public static class AutoMapperExtensions
    //    {
    //        // https://github.com/aspnet/Mvc/pull/4391/commits/f638c051fa79c293ea7a0a859ed9700db9a18d71

    //        /// <summary>
    //        /// Automatically register all <see cref="Profile"/>'s that can be located by using the <see cref="ApplicationPartManager"/>
    //        /// </summary>
    //        /// <param name="services"></param>
    //        public static void UseAutoMapper(this IServiceCollection services)
    //        {
    //            services.AddSingleton(p => new MapperConfiguration(cfg => cfg.RegisterConfigurations(p.GetService<ApplicationPartManager>().ApplicationParts)));
    //            services.AddSingleton(p => p.GetService<MapperConfiguration>().CreateMapper());
    //        }

    //        private static void RegisterConfigurations(this IMapperConfigurationExpression config, IEnumerable<ApplicationPart> applicationParts)
    //        {
    //            if (applicationParts == null)
    //                throw new ArgumentNullException(nameof(applicationParts));

    //            applicationParts.OfType<AssemblyPart>()
    //                .SelectMany(p => p.Types)
    //                .Where(x => x.IsSubclassOf(typeof(Profile)))
    //                .Where(x => x.GetConstructor(Type.EmptyTypes) != null); // Make sure it has a default parameterless constructor
    //                //.Select(Activator.CreateInstance)
    //                //.OfType<Profile>()
    //                //.ForEach(config.AddProfile);
    //        }
    //    }
    //}
}
