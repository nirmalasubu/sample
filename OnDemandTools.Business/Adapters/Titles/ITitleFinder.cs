﻿using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using System.Collections.Generic;

namespace OnDemandTools.Business.Adapters.Titles
{
    public interface ITitleFinder
    {
        IList<Title> Find(string terms);
    }
}
