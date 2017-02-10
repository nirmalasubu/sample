using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Job.Model
{
    public class Job
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime LastRunDateTime { get; set; }
        public string JobName { get; set; }
        public int Limit { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int AgentId { get; set; }

        [BsonIgnoreIfNull]
        public String LastProcessedTitleBSONId { get; set; }
    }
}
