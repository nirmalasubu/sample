using System;
using Hangfire;
using OnDemandTools.Business.Modules.Reporting;
using Serilog;

namespace OnDemandTools.Jobs.JobRegistry.Deporter
{
    public class DfStatusDeporter
    {
        private readonly IDfStatusDeporterService _deporterService;
        //resolve all concrete implementations in constructor        
        private readonly ILogger _logger;

        public DfStatusDeporter(ILogger logger, IDfStatusDeporterService deporterService)
        {
            _logger = logger;
            _deporterService = deporterService;
        }

        [AutomaticRetry(Attempts = 0)]
        public void Execute()
        {
            try
            {
                _logger.Information("started DF status deporter job");
                _deporterService.DeportDfStatuses();
                _logger.Information("ending DF status deporter job");
            }
            catch (Exception e)
            {
                _logger.Error(e, string.Format("Error in DF status Deporter Job : {0}", e));
                throw;
            }
        }
    }
}