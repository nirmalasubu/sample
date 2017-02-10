using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Jobs.JobRegistry.Models;
using System.Collections.Generic;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public interface IEnvelopeDistributor
    {
        void Distribute(IList<Envelope> envelopes, Queue deliveryQueue, DeliveryDetails details);
    }
}
