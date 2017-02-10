using OnDemandTools.Business.Modules.ModifiedTitles;
using OnDemandTools.DAL.Modules.Job;
using OnDemandTools.DAL.Modules.Job.Model;
using OnDemandTools.DAL.Modules.Job.Queries;
using OnDemandTools.DAL.Modules.Queue.Command;
using OnDemandTools.DAL.Modules.Queue.Model;
using OnDemandTools.DAL.Modules.Queue.Queries;
using System;
using System.Linq;
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
            logger.Information("started titlesync job");
            try
            {
                logger.Information("####################Registering title synchronization process#####################################");

                // Register title sync job. If it already exist, do nothing.
                logger.Information("Registering title sync job");
                JobDataModel jb = new JobDataModel()
                {
                    JobName = DAL.Modules.Job.Jobs.TitleSync.ToString(),
                    CreateDateTime = DateTime.UtcNow,
                    LastRunDateTime = DateTime.UtcNow,
                    LastProcessedTitleBSONId = svc.GetLastModifiedTitleIdOnOrBefore(DateTime.UtcNow)
                };
                jb = _jobCommand.RegisterTitleSyncJob(jb);
                logger.Information("Successfully registered title sync job");

                logger.Information("####################Completed registration of title synchronization process##########################");

                while (true)
                {
                    try
                    {
                        logger.Information("####################Started title synchronization process#####################################");

                        // Retrieve all active queues
                        logger.Information("Retrieving queues that are active");
                        var queues = _queueQuery.Get().Where(q => q.Active == true);
                        logger.Information("Retrieved {0} active queues for processing", queues.Count());

                        // Try to retieve title sync job information. Our assumption is that there will only be one title sync job
                        logger.Information("Retrieving title sync job information");
                        var job = _jobQuery.Get(DAL.Modules.Job.Jobs.TitleSync.ToString());
                        logger.Information("Successfully retrieved title sync job information");

                        // From the list of active queues, retrieve those queues that are subscribed for title change notification.
                        // Proceed to send notification to those queues
                        logger.Information("Attempting to send title change notification to subscribed queues");
                        job.LastProcessedTitleBSONId = svc.Update(queues.Where(q => q.DetectTitleChanges), job.LastProcessedTitleBSONId, job.Limit);
                        logger.Information("Successfully sent title change notification to subscribed queues");

                        // Update run status                
                        logger.Information("Attempting to update last run time and last processed title BSON id");
                        _jobCommand.UpdateTitleJobStats(job.LastProcessedTitleBSONId);
                        logger.Information("Successfully updated last run time and last processed title BSON id");

                        logger.Information("####################Completed title synchronization process###################################");

                        // Wait for sometime
                        Thread.Sleep(60000);
                    }
                    catch (Exception ex)
                    {
                        logger.Information("Error:{0}", ex.Message);
                        logger.Information("####################Abruptly ended title synchronization process##############################");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Information("Error:{0}", ex.Message);
                logger.Information("####################Abruptly ended title synchronization process##############################");
            }
            logger.Information("ending titlesync job");
        }
    }
}
