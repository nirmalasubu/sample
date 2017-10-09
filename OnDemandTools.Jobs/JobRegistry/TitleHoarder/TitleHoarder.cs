using Microsoft.Extensions.Caching.Distributed;
using OnDemandTools.Business.Modules.Airing;
using OnDemandTools.Business.Modules.Job;
using OnDemandTools.Business.Modules.Job.Model;
using OnDemandTools.Business.Modules.ModifiedTitles;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Jobs.Helpers;
using System;
using System.Linq;
using System.Text;

namespace OnDemandTools.Jobs.JobRegistry.TitleSync
{
    public class TitleHoarder
    {
        private readonly IDistributedCache _distributedCache;
        //resolve all concrete implementations in constructor
        IModifiedTitlesService svc;
        ITitleJobService _titleJobService;
        IAiringService _airingService;
        Serilog.ILogger logger;
        StringBuilder jobInfo = new StringBuilder();
       

        public TitleHoarder(IModifiedTitlesService svc,
                         Serilog.ILogger logger,
                         ITitleJobService titleJobService,
                         IAiringService airingService,
                         IDistributedCache distributedCache)
        {
            this.svc = svc;
            this.logger = logger;
            _titleJobService = titleJobService;
            _airingService = airingService;
            _distributedCache = distributedCache;
        }

        public void Execute()
        {
            jobInfo.AppendWithTime("started title hoarder job");

            try
            {
                try
                {
                    var titles = _airingService.GetTitlesInfo();

                    foreach (var title in titles)
                    {
                        _distributedCache.SetString(title.TitleId.ToString(), DateTime.Now.ToString());
                    }

                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Abruptly ended title hoarder process");
                }
            }
            finally
            {
                jobInfo.AppendWithTime("ending titlesync job");

                logger.Information(jobInfo.ToString());
            }

        }
    }
}
