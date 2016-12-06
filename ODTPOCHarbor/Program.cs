using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using FluentScheduler;
using static System.Console;


namespace ODTPOCHarbor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JobManager.Initialize(new JobRegistry());
            JobManager.JobException += (info) => WriteLine("An error just happened with a scheduled job: " + info.Exception);


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();

        }
    }
}
