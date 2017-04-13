using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Common.DIResolver.Resolvers;
using OnDemandTools.Common.Logzio;
using OnDemandTools.DAL.Modules.Airings.Queries;
using RestSharp;
using Serilog;
using StructureMap;
using OnDemandTools.Business.Modules.Airing;

namespace OnDemandTools.Jobs.Tests.Helpers
{
    public class JobTestFixture : IDisposable
    {
        public JobTestFixture()
        {
            BuildJobSettings();
            RestClient.AddDefaultHeader("Authorization", Configuration["TesterAPIKey"]);
        }


        public JobTestFixture(string apiKeyName)
        {
            BuildJobSettings();
            RestClient.AddDefaultHeader("Authorization", Configuration[apiKeyName]);
        }

        public RestClient RestClient { get; set; }
        public RestClient JobRestClient { get; set; }
        public IConfigurationRoot Configuration { get; set; }
        public IConfigurationBuilder Builder { get; set; }
        public Container Container { get; set; }
        public List<string> ProcessedAiringIds { get; set; }


        public void Dispose()
        {
            CleanupTestAirings(ProcessedAiringIds);
            RestClient = null;
            JobRestClient = null;
        }

        private void CleanupTestAirings(List<string> processedAiringIds)
        {
            var airingService = Container.GetInstance<IAiringService>();
            airingService.PurgeUnitTestAirings(processedAiringIds);
        }


        private void BuildJobSettings()
        {
            ProcessedAiringIds = new List<string>();

            // Build configuration
            Builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = Builder.Build();
            var appSettings = Configuration.Get<AppSettings>("Application");

            // Setup rest client
            RestClient = new RestClient(Configuration["APIEndPoint"]);
            RestClient.AddDefaultHeader("Content-Type", "application/json");


            // Setup Job rest client
            JobRestClient = new RestClient(Configuration["JobEndPoint"]);
            JobRestClient.AddDefaultHeader("Content-Type", "application/json");


            // Start up DI container
            Container = new Container();
            ILogger appLogger = new LoggerConfiguration()
                .WriteTo.Logzio(appSettings.LogzIO.AuthToken,
                    appSettings.LogzIO.Application,
                    appSettings.LogzIO.ReporterType,
                    appSettings.LogzIO.Environment)
                .CreateLogger();
            Container.Configure(c => c.ForSingletonOf<AppSettings>().Use(appSettings));
            Container.Configure(c => c.For<IApplicationContext>().Use<JobContext>());
            Container.Configure(c => c.ForSingletonOf<ILogger>().Use(appLogger));

            DependencyResolver.RegisterResolver(new StructureMapIOCContainer(Container)).RegisterImplmentation();

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