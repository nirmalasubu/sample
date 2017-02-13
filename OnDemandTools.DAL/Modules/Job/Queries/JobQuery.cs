using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.DAL.Modules.Job.Model;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.DAL.Modules.Job.Queries
{
    public class JobQuery : IJobQuery, IJobLastRunQuery
    {
        private readonly MongoDatabase _database;
        private readonly AppSettings _appSettings;

        public JobQuery(IODTDatastore connection, AppSettings appSettings)
        {
            _database = connection.GetDatabase();
            _appSettings = appSettings;
        }

        public JobDataModel Get(string name)
        {
            var collection = _database.GetCollection<JobDataModel>("Job");

            var job = collection.FindOne(Query.EQ("JobName", name));

            return job ?? new JobDataModel() { JobName = name };
        }

        /// <summary>
        /// Gets the job by the specified name and agent id
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns></returns>
        public JobDataModel Get(string jobName, int agentId)
        {
            var query = Query.And(Query.EQ("JobName", jobName), Query.EQ("AgentId", agentId));
            var collection = _database.GetCollection<JobDataModel>("Job");
            return collection.FindOne(query);
        }

        public IEnumerable<JobDataModel> GetHealthyJobs(string name)
        {
            var lifetimeInMinutes = int.Parse(_appSettings.HealthAgentLifetimeInMinutes) * -1;

            return GetJobs(name).Where(a => a.LastRunDateTime > DateTime.UtcNow.AddMinutes(lifetimeInMinutes));
        }

        public IEnumerable<JobDataModel> GetJobs(string name)
        {
            var collection = _database.GetCollection<JobDataModel>("Job");

            var agents = collection.Find(Query.EQ("JobName", name));

            return agents;
        }
    }
}
