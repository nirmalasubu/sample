using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Change
{
    public class FieldChange : Change
    {
        public FieldDetail Details { get; set; }

        public FieldChange()
        {
            Details = new FieldDetail();
            IsChangeDetailed = true;
        }
    }
}
