namespace OnDemandTools.API
{
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        /// <summary>
        /// Main execution entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Host the service
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://0.0.0.0:5000")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}