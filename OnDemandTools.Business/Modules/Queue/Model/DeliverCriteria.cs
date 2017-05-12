using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Queue.Model
{
    public class DeliverCriteria
    {
        public string Name { get; set; }
        public string Query { get; set; }

        public int WindowOption { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
