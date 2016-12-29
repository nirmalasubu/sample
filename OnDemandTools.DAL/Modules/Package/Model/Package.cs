using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnDemandTools.Common.Model;

namespace OnDemandTools.DAL.Modules.Package.Model
{
    [BsonIgnoreExtraElements]
    public class Package : IModel
    {
        [BsonId]
        public virtual ObjectId Id { get; set; }

        public IList<int> TitleIds { get; set; }

        //[BsonDefaultValue("")]
        [BsonIgnoreIfDefault]
        [BsonIgnoreIfNull]
        public virtual string DestinationCode { get; set; }

        public virtual string Type { get; set; }

        public BsonDocument PackageData { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public Package()
        {
            TitleIds = new List<int>();
        }
    }
}