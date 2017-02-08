using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating
{
    public interface IAiringValidatorStep
    {
        ValidationResult Validate(Airing airing, string remoteQueueName);
    }
}