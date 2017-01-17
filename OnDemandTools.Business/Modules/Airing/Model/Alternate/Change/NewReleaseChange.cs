using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Change
{
    public class NewReleaseChange : Change
    {
        public NewReleaseChange()
        {
            TheChange = "New Release";
            IsChangeDetailed = false;
        }
    }
}
