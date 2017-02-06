using OnDemandTools.Business.Modules.Queue;
using System;
using System.Threading;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        IQueueService queueService;

        public Publisher(Serilog.ILogger logger, IQueueService queueService)
        {
            this.logger = logger;
            this.queueService = queueService;
        }

        public void Execute(string queueName)
        {
            logger.Information("started publisher job for queue:" + queueName);

            var queue = queueService.GetByApiKey(queueName);

            if (queue != null && !queue.Active)
            {
                logger.Information("No Active found for queue name: {0}", queueName);
                return;
            }

            logger.Information("Publisher job completed for queue:" + queueName);
        }
    }
}
