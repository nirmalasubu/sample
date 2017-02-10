using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.Job.Model
{
    public class JobDataModel
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

    public class AgentDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LastRunDateTime { get; set; }
    }
}