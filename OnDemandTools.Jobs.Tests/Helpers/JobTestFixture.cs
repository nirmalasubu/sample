using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Common.DIResolver.Resolvers;
using OnDemandTools.Common.Logzio;
using RestSharp;
using Serilog;
using StructureMap;

namespace OnDemandTools.Jobs.Tests.Helpers
{
    public class JobTestFixture : IDisposable
    {
        public JobTestFixture()
        {
            BuildJobSettings();
            restClient.AddDefaultHeader("Authorization", Configuration["TesterAPIKey"]);
            ;
        }


        public JobTestFixture(string apiKeyName)
        {
            BuildJobSettings();
            restClient.AddDefaultHeader("Authorization", Configuration[apiKeyName]);
        }

        public RestClient restClient { get; set; }
        public RestClient jobRestClient { get; set; }
        public IConfigurationRoot Configuration { get; set; }
        public IConfigurationBuilder builder { get; set; }
        public Container container { get; set; }

        public void Dispose()
        {
            restClient = null;
        }


        private void BuildJobSettings()
        {
            // Build configuration
            builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            var appSettings = Configuration.Get<AppSettings>("Application");

            // Setup rest client
            restClient = new RestClient(Configuration["APIEndPoint"]);
            restClient.AddDefaultHeader("Content-Type", "application/json");


            // Setup Job rest client
            jobRestClient = new RestClient(Configuration["JobEndPoint"]);
            jobRestClient.AddDefaultHeader("Content-Type", "application/json");


            // Start up DI container
            container = new Container();
            ILogger appLogger = new LoggerConfiguration()
                .WriteTo.Logzio(appSettings.LogzIO.AuthToken,
                    appSettings.LogzIO.Application,
                    appSettings.LogzIO.ReporterType,
                    appSettings.LogzIO.Environment)
                .CreateLogger();
            container.Configure(c => c.ForSingletonOf<AppSettings>().Use(appSettings));
            container.Configure(c => c.For<IApplicationContext>().Use<JobContext>());
            container.Configure(c => c.ForSingletonOf<ILogger>().Use(appLogger));

            DependencyResolver.RegisterResolver(new StructureMapIOCContainer(container)).RegisterImplmentation();

            // Load mapping            
            LoadAutoMapper();
        }

        private void LoadAutoMapper()
        {
            // Load all libraries that under our namespace
            var odtLibraries = GetReferencingAssemblies("OnDemandTools")
                .SelectMany(assembly => assembly.ExportedTypes)
                .ToList();

            // Retrieve all class that are of type 'profile'
            var profiles = odtLibraries.Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                .Where(t => !t.GetTypeInfo().IsAbstract);

            // Initialize mapper profile
            Mapper.Initialize(cfg =>
                              {
                                  foreach (var profile in profiles)
                                      cfg.AddProfile(profile);
                              });
        }

        private bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Name == assemblyName
                   || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            return assemblies;
        }
    }
}