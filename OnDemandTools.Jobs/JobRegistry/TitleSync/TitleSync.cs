using OnDemandTools.Business.Modules.ModifiedTitles;
using OnDemandTools.DAL.Modules.Job;
using OnDemandTools.DAL.Modules.Job.Model;
using OnDemandTools.DAL.Modules.Job.Queries;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.Queue.Queries;
using OnDemandTools.Jobs.Helpers;
using System;
using System.Linq;
using System.Text;
using System.Threading;


namespace OnDemandTools.Jobs.JobRegistry.TitleSync
{
    public class TitleSync
    {
        //resolve all concrete implementations in constructor
        IModifiedTitlesService svc;
        QueueQuery _queueQuery;
        IJobQuery _jobQuery;
        IJobCommand _jobCommand;
        Serilog.ILogger logger;
        StringBuilder jobInfo = new StringBuilder();
        public TitleSync(IModifiedTitlesService svc, Serilog.ILogger logger, IJobQuery jobQuery, IJobCommand jobCommand, QueueQuery queueQuery)
        {
            this.svc = svc;
            this.logger = logger;
            this._jobCommand = jobCommand;
            this._jobQuery = jobQuery;
            this._queueQuery = queueQuery;
        }

        public void Execute()
        {
            jobInfo.AppendWithTime("started titlesync job");

            try
            {
                jobInfo.AppendWithTime("####################Registering title synchronization process#####################################");

                // Register title sync job. If it already exist, do nothing.
                jobInfo.AppendWithTime("Registering title sync job");
                JobDataModel jb = new JobDataModel()
                {
                    JobName = DAL.Modules.Job.Jobs.TitleSync.ToString(),
                    CreateDateTime = DateTime.UtcNow,
                    LastRunDateTime = DateTime.UtcNow,
                    LastProcessedTitleBSONId = svc.GetLastModifiedTitleIdOnOrBefore(DateTime.UtcNow)
                };
                jb = _jobCommand.RegisterTitleSyncJob(jb);
                jobInfo.AppendWithTime("Successfully registered title sync job");

                jobInfo.AppendWithTime("####################Completed registration of title synchronization process##########################");


                try
                {
                    jobInfo.AppendWithTime("####################Started title synchronization process#####################################");

                    // Retrieve all active queues
                    jobInfo.AppendWithTime("Retrieving queues that are active");
                    var queues = _queueQuery.Get().Where(q => q.Active == true);
                    jobInfo.AppendWithTime(string.Format("Retrieved {0} active queues for processing", queues.Count()));

                    // Try to retieve title sync job information. Our assumption is that there will only be one title sync job
                    jobInfo.AppendWithTime("Retrieving title sync job information");
                    var job = _jobQuery.Get(DAL.Modules.Job.Jobs.TitleSync.ToString());
                    jobInfo.AppendWithTime("Successfully retrieved title sync job information");

                    // From the list of active queues, retrieve those queues that are subscribed for title change notification.
                    // Proceed to send notification to those queues
                    jobInfo.AppendWithTime("Attempting to send title change notification to subscribed queues");
                    job.LastProcessedTitleBSONId = svc.Update(queues.Where(q => q.DetectTitleChanges), job.LastProcessedTitleBSONId, job.Limit);
                    jobInfo.AppendWithTime("Successfully sent title change notification to subscribed queues");

                    // Update run status                
                    jobInfo.AppendWithTime("Attempting to update last run time and last processed title BSON id");
                    _jobCommand.UpdateTitleJobStats(job.LastProcessedTitleBSONId);
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
