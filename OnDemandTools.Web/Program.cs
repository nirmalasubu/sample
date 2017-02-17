using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;


namespace OnDemandTools.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Set default environment to development unless indicated in environment variable
                var defaults = new Dictionary<string, string> { { WebHostDefaults.EnvironmentKey, "Development" } };

                var config = new ConfigurationBuilder()
                      .AddInMemoryCollection(defaults)
                      .AddCommandLine(args)
                      .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                      .Build();


                var host = new WebHostBuilder()
                        .UseConfiguration(config)
                        .UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .Build();

                host.Run();
            }
            catch (Exception ex)
            {

                Console.Write("Unhandled exception occurred. Shutting down..." + ex.StackTrace);
            }
        }
    }
}
