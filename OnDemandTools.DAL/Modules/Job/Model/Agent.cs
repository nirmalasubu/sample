using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Job.Model
{
    public class Agent
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LastRunDateTime { get; set; }
    }
}
