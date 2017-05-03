using OnDemandTools.Business.Modules.HangFire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.HangFire
{
   public interface IGetHangireServers
    {
        List<HangfireServerModel> Get();
        HangFireStatusModel GetStatus();
    }
}
