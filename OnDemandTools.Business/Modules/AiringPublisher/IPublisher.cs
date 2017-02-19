namespace OnDemandTools.Business.Modules.AiringPublisher
{
    public interface IPublisher
    {
        void Execute(string queueName);
    }
}
