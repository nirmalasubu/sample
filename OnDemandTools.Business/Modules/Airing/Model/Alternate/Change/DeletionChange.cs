using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Change
{
    public class DeletionChange : Change
    {
        public DeletionChange()
        {
            TheChange = "Deletion";
            IsChangeDetailed = false;
        }
    }
}
