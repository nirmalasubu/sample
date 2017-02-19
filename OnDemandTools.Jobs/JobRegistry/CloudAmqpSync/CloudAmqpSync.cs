using Hangfire;
using OnDemandTools.Business.Modules.AiringPublisher.Workflow;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Configuration;
using System;
using System.Threading;

namespace OnDemandTools.Jobs.JobRegistry.CloudAmqpSync
{
    /// <summary>
    /// This job creates the missing queues in Cloud AMPQ Server
    /// </summary>
    public class CloudAmqpSync
    {
        private readonly IQueueService queueService;
        private readonly AppSettings appsettings;
        private readonly IRemoteQueueHandler remoteQueueHandler;
        private readonly Serilog.ILogger logger;
        public CloudAmqpSync(
            Serilog.ILogger logger,
            IQueueService queueService,
            AppSettings appsettings,
            IRemoteQueueHandler remoteQueueHandler)
        {
            this.appsettings = appsettings;
            this.queueService = queueService;
            this.remoteQueueHandler = remoteQueueHandler;
        }

        [AutomaticRetry(Attempts = 0)]
        public void Execute()
        {
            foreach (var activeQueue in queueService.GetByStatus(true))
            {
                try
                {
                    remoteQueueHandler.Create(activeQueue);
                    Thread.Sleep(1000);
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "Unexpected error occured while creating cloud AMQP queue");
                }
            }
        }
    }
}
