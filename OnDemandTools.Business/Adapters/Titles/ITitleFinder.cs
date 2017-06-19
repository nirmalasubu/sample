using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Adapters.Titles
{
    public interface ITitleFinder
    {
        IList<int> Find(string terms);
    }
}
