using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Business.Modules.Queue;
using System.Linq;


namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators
{
    public class MessageDeliveryValidator : IMessageDeliveryValidator
    {
        private readonly IQueueService queueService;

        public MessageDeliveryValidator(IQueueService queueService)
        {
            this.queueService = queueService;
        }

        #region IMessageDeliveryValidator Members

        public bool Validate(Airing airing, string queueName)
        {
            return queueService.AnyMessageDeliveredForAiringId(airing.AssetId, queueName);
        }

        #endregion
    }
}