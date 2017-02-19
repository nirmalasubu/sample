using BLQueue = OnDemandTools.Business.Modules.Queue.Model;
using BLAiring = OnDemandTools.Business.Modules.Airing.Model;
using System.Collections.Generic;
using OnDemandTools.Business.Modules.AiringPublisher.Models;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public interface IEnvelopeStuffer
    {
        List<Envelope> Generate(IList<BLAiring.Airing> airings, BLQueue.Queue queue, Action action);
    }
}