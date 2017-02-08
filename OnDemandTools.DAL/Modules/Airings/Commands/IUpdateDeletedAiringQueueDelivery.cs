namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public interface IUpdateDeletedAiringQueueDelivery
    {
        void PushDeliveredTo(string airingId, string queueName);

        void PushIgnoredQueueTo(string airingId, string queueName);
    }
}
