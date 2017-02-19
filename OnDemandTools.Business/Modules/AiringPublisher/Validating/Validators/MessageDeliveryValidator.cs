using BLModel= OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Business.Modules.Queue;
using System.Linq;


namespace OnDemandTools.Business.Modules.AiringPublisher.Validating.Validators
{
    public class MessageDeliveryValidator : IMessageDeliveryValidator
    {
        private readonly IQueueService queueService;

        public MessageDeliveryValidator(IQueueService queueService)
        {
            this.queueService = queueService;
        }

        #region IMessageDeliveryValidator Members

        public bool Validate(BLModel.Airing airing, string queueName)
        {
            return queueService.AnyMessageDeliveredForAiringId(airing.AssetId, queueName);
        }

        #endregion
    }
}