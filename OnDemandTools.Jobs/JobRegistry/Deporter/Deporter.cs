using Microsoft.Extensions.Configuration;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Common.Configuration;
using System;
using System.Threading;


namespace OnDemandTools.Jobs.JobRegistry.Deporter
{
    public class Deporter
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        IAiringService airingServiceHelper;
        private IConfiguration configuration { get; }

        public Deporter(Serilog.ILogger logger,IAiringService airingService, IConfiguration configuration)
        {

            this.configuration = configuration;
            this.logger = logger;
            this.airingServiceHelper = airingService;
        }

        public void Execute()
        {
            try
            {
                logger.Information("started deporter job");
                airingServiceHelper.Deport(int.Parse(configuration.Get("airingDeportGraceDays")));
                logger.Information("ending deporter job");
            }
            catch(Exception e)
            {
                logger.Error(string.Format("Error in Deporter Job : {0}", e));
            }
        }
    }
}
