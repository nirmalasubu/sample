using OnDemandTools.Business.Modules.Airing;
using System;
using System.Threading;


namespace OnDemandTools.Jobs.JobRegistry.TitleSync
{
    public class TitleSync
    {
        //resolve all concrete implementations in constructor
        IAiringService svc;
        Serilog.ILogger logger;
        public TitleSync(IAiringService svc, Serilog.ILogger logger)
        {
           this.svc = svc;
            this.logger = logger;
        }

        public void Execute()
        {
            logger.Information("started titlesync job");
            Thread.Sleep(3000);
            logger.Information("ending titlesync job");
        }
    }
}
