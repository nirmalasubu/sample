using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Nancy;
using Nancy.TinyIoc;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Diagnostics;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Common.DIResolver
{
    public class TinyIOCResolver : IDependencyResolver
    {
        private TinyIoCContainer cntr;


        public TinyIOCResolver(TinyIoCContainer underlyingContainer)
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

            foreach (var item in profiles)
            {
                var ints = item.GetInterfaces();
                foreach (var it in ints)
                {
                    if (it.ToString().StartsWith("OnDemandTools", StringComparison.OrdinalIgnoreCase))
                    {
                        cntr.Register(it, item);
                    }

                }
            }

            // Add those that cannot be auto registered. Basically the ones outside 'OnDemandTools' namespace or
            // someother special case   
            cntr.Register(typeof(ISerializer), typeof(OnDemandTools.API.Utilities.Serialization.CustomJsonSerializer));          


            // Special initialization for StatusLibrary class
            OnDemandTools.DAL.Modules.Reporting.Library.StatusLibrary.Init(cntr.Resolve<AppSettings>());

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