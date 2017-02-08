using System;
using MongoDB.Bson;

namespace OnDemandTools.DAL.Modules.QueueMessages.Model
{
    public class HistoricalMessage
    {
        public HistoricalMessage(string airingId, string mediaId, string message, string remoteQueueName, byte? messagePriority)
        {
            DateTime = DateTime.UtcNow;
            AiringId = airingId;
            RemoteQueueName = remoteQueueName;
            Message = message;
            MediaId = mediaId;
            MessagePriority = messagePriority;
        }

        public ObjectId _id { get; set; }
        public string AiringId { get; set; }
        public string RemoteQueueName { get; set; }
        public byte? MessagePriority { get; set; }
        public string Message { get; set; }
        public string MediaId { get; set; }
        public DateTime DateTime { get; set; }
    }
}