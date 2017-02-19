using BLQueue = OnDemandTools.Business.Modules.Queue.Model;
using BLAiring = OnDemandTools.Business.Modules.Airing.Model;
using System.Collections.Generic;
using OnDemandTools.Business.Modules.AiringPublisher.Models;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class EnvelopeStuffer : IEnvelopeStuffer
    {
        private readonly IMessagePriorityCalculator _priorityCalculator;
        public EnvelopeStuffer(IMessagePriorityCalculator priorityCalculator)
        {
            _priorityCalculator = priorityCalculator;
        }

        public List<Envelope> Generate(IList<BLAiring.Airing> airings, BLQueue.Queue queue, Action action)
        {
            var packager = new QueuePackager();

            var envelopes = new List<Envelope>();

            foreach (var airing in airings)
            {
                var envelope = new Envelope
                                   {
                                       AiringId = airing.AssetId,
                                       PostMarkedDateTime = airing.ReleaseOn,
                                       MessagePriority = _priorityCalculator.Calculate(queue, airing),
                                       Message = packager.Package(airing, action),
                                       MediaId = airing.MediaId
                                   };

                envelopes.Add(envelope);
            }

            return envelopes;
        }
    }
}