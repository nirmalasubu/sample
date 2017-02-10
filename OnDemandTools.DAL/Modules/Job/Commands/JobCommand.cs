using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Job.Model;
using System;

namespace OnDemandTools.DAL.Modules.Job.Commands
{
    public class JobCommand : IJobCommand
    {
        private readonly MongoCollection<JobDataModel> _jobCollection;
        private readonly MongoCollection<AgentDataModel> _agentCollection;

        public JobCommand(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _jobCollection = database.GetCollection<JobDataModel>("Job");
            _agentCollection = database.GetCollection<AgentDataModel>("JobAgent");
        }

        /// <summary>
        /// Registers the job with agent information
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Error saving job:  + job.JobName +  - + job.AgentId.ToString()</exception>
        public JobDataModel RegisterJobCumAgent(JobDataModel job)
        {
            var query = Query.And(Query.EQ("JobName", job.JobName), Query.EQ("AgentId", job.AgentId));
            var update = Update<JobDataModel>
                .Set(c => c.AgentId, job.AgentId)
                .Set(c => c.JobName, job.JobName)
                .Set(c => c.CreateDateTime, DateTime.UtcNow)
                .Set(c => c.LastRunDateTime, DateTime.UtcNow)
                .Set(c => c.Limit, job.Limit);

            WriteConcernResult wr = _jobCollection.Update(query, update, UpdateFlags.Upsert);
            if (wr.DocumentsAffected >= 1)
                return _jobCollection.FindOne(query);
            else
                throw new Exception("Error saving job: " + job.JobName + " - "+ job.AgentId.ToString());

        }

        /// <summary>
        /// Registers the agent.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Error saving agent:  + agent.AgentName +  -  + agent.AgentId.ToString()</exception>
        public AgentDataModel RegisterAgent(AgentDataModel agent)
        {

            var query = Query.And(Query.EQ("AgentName", agent.AgentName), Query.EQ("AgentId", agent.AgentId));
            var update = Update<AgentDataModel>
                .Set(c => c.AgentId, agent.AgentId)
                .Set(c => c.AgentName, agent.AgentName)
                .Set(c => c.CreateDateTime, DateTime.UtcNow)
                .Set(c => c.LastRunDateTime, DateTime.UtcNow);

            WriteConcernResult wr = _agentCollection.Update(query, update, UpdateFlags.Upsert);
            if (wr.DocumentsAffected >= 1)
                return _agentCollection.FindOne(query);
            else
                throw new Exception("Error saving agent: " + agent.AgentName + " - " + agent.AgentId.ToString());
        }

        public void UpdateJobLastRunDateTime(JobDataModel job)
        {
            var query = Query.And(Query.EQ("JobName", job.JobName), Query.EQ("AgentId", job.AgentId));
            _jobCollection.Update(query, Update.Set("LastRunDateTime", DateTime.UtcNow));
        }

        public void UpdateJobLastRunDateTime(String jobName)
        {
            var query = Query.EQ("JobName", jobName);
            _jobCollection.Update(query, Update.Set("LastRunDateTime", DateTime.UtcNow));
        }

        public void UpdateTitleJobStats(String lastTitleBSONId)
        {
            var query = Query.EQ("JobName", Jobs.TitleSync.ToString());
            var update = Update<JobDataModel>
               .Set(c => c.LastRunDateTime, DateTime.UtcNow)
               .Set(c => c.LastProcessedTitleBSONId, lastTitleBSONId);
            _jobCollection.Update(query, update);
        }


        public void UpdateDeporterJobStats()
        {
            var query = Query.EQ("JobName", Jobs.ExpiredAiringDeporter.ToString());
            var update = Update<JobDataModel>
               .Set(c => c.LastRunDateTime, DateTime.UtcNow);
               
            _jobCollection.Update(query, update);
        }


        public void UpdateAgentLastRunDateTime(AgentDataModel agent)
        {
            var query = Query.And(Query.EQ("AgentName", agent.AgentName), Query.EQ("AgentId", agent.AgentId));
            _agentCollection.Update(query, Update.Set("LastRunDateTime", DateTime.UtcNow));
        }


        /// <summary>
        /// Registers the job if it doesn't exist. If it already exist, do nothing
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobDataModel RegisterTitleSyncJob(JobDataModel job)
        {
            var query = Query.And(Query.EQ("JobName", job.JobName));
            var update = Update<JobDataModel>
                .SetOnInsert(c => c.JobName, job.JobName)
                .SetOnInsert(c => c.CreateDateTime, job.CreateDateTime)
                .SetOnInsert(c => c.LastRunDateTime, job.LastRunDateTime)
                .SetOnInsert(c=> c.LastProcessedTitleBSONId, job.LastProcessedTitleBSONId);

            WriteConcernResult wr = _jobCollection.Update(query, update, UpdateFlags.Upsert);
            return _jobCollection.FindOne(query);
        }



        /// <summary>
        /// Registers reporter job if it doesn't exist. If it already exist, do nothing
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JobDataModel RegisterDeporterJob(JobDataModel job)
        {
            var query = Query.And(Query.EQ("JobName", job.JobName));
            var update = Update<JobDataModel>
                .SetOnInsert(c => c.JobName, job.JobName)
                .SetOnInsert(c => c.CreateDateTime, job.CreateDateTime)
                .SetOnInsert(c => c.LastRunDateTime, job.LastRunDateTime);
                
            WriteConcernResult wr = _jobCollection.Update(query, update, UpdateFlags.Upsert);
            return _jobCollection.FindOne(query);
        }

    }
}