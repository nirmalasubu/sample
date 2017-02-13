using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Jobs.JobRegistry.Models;
using System.Collections.Generic;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class EnvelopeStuffer : IEnvelopeStuffer
    {
        private readonly IMessagePriorityCalculator _priorityCalculator;
        public EnvelopeStuffer(IMessagePriorityCalculator priorityCalculator)
        {
            _priorityCalculator = priorityCalculator;
        }

        public List<Envelope> Generate(IList<Airing> airings, Queue queue, Action action)
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