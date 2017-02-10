using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTools.DAL.Modules.Job.Model;

namespace OnDemandTools.DAL.Modules.Job.Queries
{
    public interface IJobQuery
    {
        JobDataModel Get(string name);

        JobDataModel Get(string jobName, int agentId);
    }
}
