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
    public class TitleSync
    {
        //resolve all concrete implementations in constructor
        IModifiedTitlesService svc;
        ITitleJobService _titleJobService;
        IQueueService _queueService;
        Serilog.ILogger logger;
        StringBuilder jobInfo = new StringBuilder();
       

        public TitleSync(IModifiedTitlesService svc,
                         Serilog.ILogger logger,
                         ITitleJobService titleJobService,
                         IQueueService queueService)
        {
            this.svc = svc;
            this.logger = logger;
            _titleJobService = titleJobService;
            _queueService = queueService;
        }

        public void Execute()
        {
            jobInfo.AppendWithTime("started titlesync job");

            try
            {
                jobInfo.AppendWithTime("#################### Registering title synchronization process#####################################");

                // Register title sync job. If it already exist, do nothing.
                string lastProcessedTitleBSONId;
                   TitleJobModel titleJobModel = _titleJobService.Get("TitleSync");
                    if (string.IsNullOrEmpty(titleJobModel.LastProcessedTitleBSONId))
                    {
                        TitleJobModel jb = new TitleJobModel()
                        {
                            JobName = "TitleSync",
                            CreateDateTime = DateTime.UtcNow,
                            LastRunDateTime = DateTime.UtcNow,
                            LastProcessedTitleBSONId = svc.GetLastModifiedTitleIdOnOrBefore(DateTime.UtcNow)
                        };
                        TitleJobModel savedJob = _titleJobService.RegisterTitleSyncJob(jb);
                        lastProcessedTitleBSONId = savedJob.LastProcessedTitleBSONId;
                    }
                    else
                    {
                        lastProcessedTitleBSONId = titleJobModel.LastProcessedTitleBSONId;
                    }
               

                jobInfo.AppendWithTime("Successfully registered title sync job");

                try
                {
                    jobInfo.AppendWithTime("####################Started title synchronization process#####################################");

                    // Retrieve all active queues
                    jobInfo.AppendWithTime("Retrieving queues that are active");
                    var queues = _queueService.GetByStatus(true);


                    jobInfo.AppendWithTime(string.Format("Retrieved {0} active queues for processing", queues.Count()));
                    // Try to retieve title sync job information. Our assumption is that there will only be one title sync job
                    jobInfo.AppendWithTime("Retrieving title sync job information");


                    // From the list of active queues, retrieve those queues that are subscribed for title change notification.
                    // Proceed to send notification to those queues
                    jobInfo.AppendWithTime("Attempting to send title change notification to subscribed queues");

                    lastProcessedTitleBSONId = svc.Update(queues.Where(q => q.DetectTitleChanges), lastProcessedTitleBSONId, 1000);
                    jobInfo.AppendWithTime("Successfully sent title change notification to subscribed queues");

                    // Update run status                
                    jobInfo.AppendWithTime("Attempting to update last run time and last processed title BSON id");
                    _titleJobService.UpdatelastTitleBSONId(lastProcessedTitleBSONId);

                    jobInfo.AppendWithTime("Successfully updated last run time and last processed title BSON id");
                    jobInfo.AppendWithTime("####################Completed title synchronization process###################################");

                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Abruptly ended title synchronization process");
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
