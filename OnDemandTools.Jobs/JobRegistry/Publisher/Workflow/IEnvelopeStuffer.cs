using OnDemandTools.Business.Modules.Queue.Model;
using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Jobs.JobRegistry.Models;
using System.Collections.Generic;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public interface IEnvelopeStuffer
    {
        List<Envelope> Generate(IList<Airing> airings, Queue queue, Action action);
    }
}