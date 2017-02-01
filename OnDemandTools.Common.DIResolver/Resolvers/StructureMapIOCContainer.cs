using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Database;

namespace OnDemandTools.Common.DIResolver.Resolvers
{
    
    public class StructureMapIOCContainer : IDependencyResolver
    {

        private StructureMap.Container cntr;

        public StructureMapIOCContainer(StructureMap.Container underlyingContainer)
        {
            this.cntr = underlyingContainer;
        }

        public void Dispose()
        {
            cntr.Dispose();
        }

        public void RegisterImplmentation()
        {
            // Auto register
            var odtLibraries = GetReferencingAssemblies("OnDemandTools")
                     .SelectMany(assembly => assembly.ExportedTypes)
                     .ToList();


            var profiles = odtLibraries.Where(t => t.GetTypeInfo().IsClass
                        && !t.GetTypeInfo().IsAbstract
                        && !t.GetTypeInfo().IsInterface).ToList();


            cntr.Configure(c =>
            {

                foreach (var item in profiles)
                {
                    var ints = item.GetInterfaces();
                    foreach (var it in ints)
                    {
                        if (it.ToString().StartsWith("OnDemandTools", StringComparison.OrdinalIgnoreCase))
                        {
                            c.AddType(it, item);                     
                        }
                    }
                }



                // Add those that cannot be auto registered. Basically the ones outside 'OnDemandTools' namespace or
                // someother special case   
                c.For<IODTDatastore>().Use<ODTPrimaryDatastore>();

                // Special initialization for StatusLibrary class
                OnDemandTools.DAL.Modules.Reporting.Library.StatusLibrary.Init((AppSettings)cntr.GetInstance(typeof(AppSettings)));


            });

            
        }



        private bool IsOnDemand(RuntimeLibrary library, String assemblyName)
        {
            return library.Name.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {

                if (IsOnDemand(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }
    }
}
