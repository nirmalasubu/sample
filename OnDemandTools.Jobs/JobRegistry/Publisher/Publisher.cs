using Hangfire;
using OnDemandTools.Business.Modules.AiringPublisher;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
        IPublisher publisher;
        public Publisher(IPublisher publisher)
        {
            this.publisher = publisher;
        }


        [AutomaticRetry(Attempts = 0)]
        public void Execute(string queueName)
        {
            publisher.Execute(queueName);
        }
    }
}
