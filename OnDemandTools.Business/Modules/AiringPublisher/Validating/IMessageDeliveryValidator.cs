using BLModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher.Validating
{
    public interface IMessageDeliveryValidator
    {
        bool Validate(BLModel.Airing airing, string queueName);
    }
}