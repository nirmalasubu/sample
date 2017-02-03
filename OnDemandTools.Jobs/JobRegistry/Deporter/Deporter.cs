using System;
using System.Threading;


namespace OnDemandTools.Jobs.JobRegistry.Deporter
{
    public class Deporter
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        public Deporter(Serilog.ILogger logger)
        {
            this.logger = logger;
        }

        public void Execute()
        {
            logger.Information("started deporter job");
            Thread.Sleep(6000);
            logger.Information("ending deporter job");
        }
    }
}
