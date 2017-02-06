using System;
using System.Threading;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
        //resolve all concrete implementations in constructor        
        Serilog.ILogger logger;
        public Publisher(Serilog.ILogger logger)
        {

            this.logger = logger;
        }

        public void Execute(string queueName)
        {
            logger.Information("started publisher job for queue:" + queueName);
            Thread.Sleep(1000);
            logger.Information("Publisher job completed for queue:" + queueName);
        }
    }
}
