using System;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.Airings.Model
{
    public class AiringLink
    {
        [BsonIgnoreIfNull]
        public DateTime? Date { get; set; }

        public int AiringId { get; set; }

        public bool Linked { get; set; }

        public string Authority { get; set; }
    }
}