using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnDemandTools.DAL.Modules.Job.Model;

namespace OnDemandTools.DAL.Modules.Job
{
    public interface IJobCommand
    {
        JobDataModel RegisterJobCumAgent(JobDataModel job);

        JobDataModel RegisterTitleSyncJob(JobDataModel job);

        JobDataModel RegisterDeporterJob(JobDataModel job);

        void UpdateJobLastRunDateTime(JobDataModel job);

        void UpdateJobLastRunDateTime(String jobName);

        void UpdateTitleJobStats(String lastTitleBSONId);

        void UpdateDeporterJobStats();

        void UpdateAgentLastRunDateTime(AgentDataModel agent);
        AgentDataModel RegisterAgent(AgentDataModel agent);
    }


    public interface IJobLastRunQuery
    {
        JobDataModel Get(string name);
        IEnumerable<AgentDataModel> GetHealthyAgents(string name);

        IEnumerable<AgentDataModel> GetAgents(string name);

        IEnumerable<JobDataModel> GetJobs(string name);

        IEnumerable<JobDataModel> GetHealthyJobs(string name);
    }
}
