using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Change
{
    public class FieldDetail
    {
        public ChangeValue Original { get; set; }
        public ChangeValue Previous { get; set; }
        public ChangeValue Current { get; set; }

        public bool HasPrevious { get { return !string.IsNullOrEmpty(Previous.Value); } }

        public FieldDetail()
        {
            Original = new ChangeValue();
            Previous = new ChangeValue();
            Current = new ChangeValue();
        }
    }
}
