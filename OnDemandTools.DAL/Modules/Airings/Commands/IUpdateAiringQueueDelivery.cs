namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public interface IUpdateAiringQueueDelivery
    {
        void PushDeliveredTo(string airingId, string queueName);
       
        void PushIgnoredQueueTo(string airingId, string queueName);

        bool IsAiringDistributed(string airingId, string queueName);
    }
}
