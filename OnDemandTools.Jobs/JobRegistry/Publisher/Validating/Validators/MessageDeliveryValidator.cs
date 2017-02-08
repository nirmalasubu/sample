using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.QueueMessages;
using System.Linq;
using OnDemandTools.DAL.Modules.QueueMessages.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators
{
    public class MessageDeliveryValidator : IMessageDeliveryValidator
    {
        private readonly IGetQueueMessagesQuery _getQueueMessages;

        public MessageDeliveryValidator(IGetQueueMessagesQuery getQueueMessages)
        {
            _getQueueMessages = getQueueMessages;
        }

        #region IMessageDeliveryValidator Members

        public bool Validate(Airing airing, string queueName)
        {
            IQueryable<HistoricalMessage> messages = _getQueueMessages.GetBy(queueName, airing.AssetId);

            return messages.Any();
        }

        #endregion
    }
}