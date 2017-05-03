using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.HangFire.Model
{
    public class HangfireServerModel
    {
        public HangfireServerModel()
        {

        }
        public string Name { get; set; }
        public DateTime Heartbeat { get; set; }
    }
}
