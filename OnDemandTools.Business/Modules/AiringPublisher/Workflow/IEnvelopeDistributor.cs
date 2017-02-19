using BLQueue = OnDemandTools.Business.Modules.Queue.Model;
using System.Collections.Generic;
using System.Text;
using OnDemandTools.Business.Modules.AiringPublisher.Models;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public interface IEnvelopeDistributor
    {
        void Distribute(IList<Envelope> envelopes, BLQueue.Queue deliveryQueue, DeliveryDetails details, StringBuilder logger);
    }
}
