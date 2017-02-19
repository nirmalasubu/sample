using BLModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.AiringPublisher.Validating
{
    public interface IAiringValidatorStep
    {
        ValidationResult Validate(BLModel.Airing airing, string remoteQueueName);
    }
}