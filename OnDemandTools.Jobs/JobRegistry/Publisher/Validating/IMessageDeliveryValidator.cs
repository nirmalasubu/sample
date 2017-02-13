using OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating
{
    public interface IMessageDeliveryValidator
    {
        bool Validate(Airing airing, string queueName);
    }
}