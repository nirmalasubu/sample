using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.QueueMessages;
using System.Linq;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators
{
    public class MediaIdValidator : IAiringValidatorStep
    {
        private readonly IGetQueueMessagesQuery _queueMessagesQuery;

        public MediaIdValidator(IGetQueueMessagesQuery queueMessagesQuery)
        {
            _queueMessagesQuery = queueMessagesQuery;
        }

        public ValidationResult Validate(Airing airing, string remoteQueueName)
        {
            if (string.IsNullOrEmpty(airing.MediaId))
                return new ValidationResult(false, 11, "MediaId is missing when it was required.", true);

            return _queueMessagesQuery.GetByMediaId(airing.MediaId, remoteQueueName).Any() ? new ValidationResult(false, 11, "MediaId already delivered to the queue.", true) : new ValidationResult(true);
        }

    }

}