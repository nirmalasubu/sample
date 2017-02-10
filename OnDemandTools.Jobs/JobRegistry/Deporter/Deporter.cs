using Hangfire;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Common.Configuration;
using System;

namespace OnDemandTools.Jobs.JobRegistry.Deporter
{
    public class Deporter
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        IAiringService airingServiceHelper;
        AppSettings appsettings;

        public Deporter(Serilog.ILogger logger, IAiringService airingService, AppSettings appsettings)
        {

            this.appsettings = appsettings;
            this.logger = logger;
            this.airingServiceHelper = airingService;
        }

        [AutomaticRetry(Attempts = 0)]        
        public void Execute()
        {
            try
            {
                logger.Information("started deporter job");
                airingServiceHelper.Deport(int.Parse(appsettings.AiringDeportGraceDays));
                logger.Information("ending deporter job");
            }
            catch (Exception e)
            {
                logger.Error(e, string.Format("Error in Deporter Job : {0}", e));
                throw;
            }
        }
    }
}
