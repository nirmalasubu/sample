using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;

using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using AutoMapper;
using OnDemandTools.Common.DIResolver;
using OnDemandTools.Common.DIResolver.Resolvers;
using OnDemandTools.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnDemandTools.Business.Modules.Airing;
using Newtonsoft.Json;

namespace OnDemandTools.Business.Tests.Helpers
{
    public class BusinessTestFixture : IDisposable
    {

        public IConfigurationRoot Configuration { get; }       
        public StructureMap.Container container { get; set; }

        public BusinessTestFixture()
        {
            // Build configuration settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
            var appSettings = Configuration.Get<AppSettings>("Application");

            // Start up DI container
            this.container = new StructureMap.Container();
            container.Configure(c => c.ForSingletonOf<AppSettings>().Use(appSettings));
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
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        private bool IsCandidateLibrary(RuntimeLibrary library, String assemblyName)
        {
            return library.Name == (assemblyName)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }

}
