using OnDemandTools.Business.Modules.Job.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Job
{
   public interface ITitleJobService
    {
        TitleJobModel Get(string name);

        TitleJobModel RegisterTitleSyncJob(TitleJobModel job);

        void UpdatelastTitleBSONId(String lastTitleBSONId);

    }
}
