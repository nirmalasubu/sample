using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OnDemandTools.API.Helpers
{
    /// <summary>
    /// Extensions for Automapper profile initializations
    /// </summary>
    public static class AutoMapperHelper
    {

        public static void InitializeAutoMapper(this IServiceCollection services)
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


        private static bool IsCandidateLibrary(RuntimeLibrary library, String assemblyName)
        {
            return library.Name == (assemblyName)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
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
    }
}
