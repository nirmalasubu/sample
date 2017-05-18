namespace OnDemandTools.Business.Adapters.Hangfire
{
    public interface IHangfireRecurringJobCommand
    {
        void CreatePublisherJob(string queueName);
        void DeletePublisherJob(string queueName);
    }
}
