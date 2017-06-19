using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Adapters.Titles
{
    public interface IFlowTitles
    {
        List<Title> GetFlowTitlesFor(IEnumerable<int> titleIds);
    }
}
