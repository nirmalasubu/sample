using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Business.Modules.Queue;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators
{
    public class MediaIdValidator : IAiringValidatorStep
    {
        private readonly IQueueService queueService;

        public MediaIdValidator(IQueueService queueService)
        {
            this.queueService = queueService;
        }

        public ValidationResult Validate(Airing airing, string remoteQueueName)
        {
            if (string.IsNullOrEmpty(airing.MediaId))
                return new ValidationResult(false, 11, "MediaId is missing when it was required.", true);

            return queueService.AnyMessageDeliveredForMediaId(airing.MediaId, remoteQueueName) ?
                new ValidationResult(false, 11, "MediaId already delivered to the queue.", true)
                : new ValidationResult(true);
        }
    }
}