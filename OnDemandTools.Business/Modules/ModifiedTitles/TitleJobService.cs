using OnDemandTools.Business.Modules.Job.Model;
using OnDemandTools.DAL.Modules.Job;
using OnDemandTools.DAL.Modules.Job.Queries;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Job.Model;

namespace OnDemandTools.Business.Modules.Job
{
    public class TitleJobService : ITitleJobService
    {
        IJobCommand _jobCommand;
        IJobQuery _jobQuery;
       public TitleJobService(IJobCommand jobCommand, IJobQuery jobQuery)
        {
            _jobCommand = jobCommand;
            _jobQuery = jobQuery;
        }


        public TitleJobModel Get(string name)
        {
            return
                (_jobQuery.Get(name)                
                .ToBusinessModel<JobDataModel, TitleJobModel>());
        }

        public TitleJobModel RegisterTitleSyncJob(TitleJobModel job)
        {
            return
          (_jobCommand.RegisterTitleSyncJob(job.ToDataModel<TitleJobModel, JobDataModel>())
              .ToBusinessModel<JobDataModel, TitleJobModel>()); 
        }

        public void UpdatelastTitleBSONId(string lastTitleBSONId)
        {
            _jobCommand.UpdateTitleJobStats(lastTitleBSONId);
        }
    }
}
