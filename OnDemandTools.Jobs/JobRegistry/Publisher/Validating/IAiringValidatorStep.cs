using OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating
{
    public interface IAiringValidatorStep
    {
        ValidationResult Validate(Airing airing, string remoteQueueName);
    }
}