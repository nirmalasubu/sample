using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating
{
    public interface IMessageDeliveryValidator
    {
        bool Validate(Airing airing, string queueName);
    }
}