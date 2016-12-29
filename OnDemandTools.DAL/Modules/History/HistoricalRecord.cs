using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OnDemandTools.Models.History
{
    public class HistoricalRecord
    {
        [BsonId]
        public virtual ObjectId Id { get; set; }

        public Common.Model.IModel Document { get; set; }
        public string Action { get; set; }
        public string User { get; set; }
        public DateTime PerformedAt { get; set; }

        public HistoricalRecord(Common.Model.IModel doc, string action, string userName)
        {
            Document = doc;
            User = userName;
            PerformedAt = DateTime.UtcNow;
            Action = action;
        }
    }
}