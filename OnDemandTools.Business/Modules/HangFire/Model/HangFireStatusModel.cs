using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.HangFire.Model
{
    public class HangFireStatusModel
    {
        public int Count { get; set; }

        public DateTime LastHeartbeat { get; set; }
    }
}
