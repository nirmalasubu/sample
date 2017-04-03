using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace OnDemandTools.Web
{
    public class Program
    {
        /// <summary>
        /// Main execution entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                // Set default environment to development unless indicated in environment variable
                var defaults = new Dictionary<string, string> { { WebHostDefaults.EnvironmentKey, "development" } };


                var config = new ConfigurationBuilder()
                .AddInMemoryCollection(defaults)
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

              if(config.GetValue<string>("environment") == "local"){

                var host = new WebHostBuilder()
                    .UseConfiguration(config)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseUrls("http://localhost:5010")
                    .Build();

                     host.Run();

              }
              else {
                 var host = new WebHostBuilder()
                    .UseConfiguration(config)
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()                    
                    .Build();

                 host.Run();
              }

               
            }
            catch (Exception ex)
            {
                Console.Write("Unhandled exception occurred. Shutting down..." + ex.StackTrace);
            }

        }
    }
}
