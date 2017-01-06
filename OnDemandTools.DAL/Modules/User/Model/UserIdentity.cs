using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnDemandTools.Common.Model;

namespace OnDemandTools.DAL.Modules.User.Model
{
    [BsonIgnoreExtraElements]
    public class UserIdentity : IModel
    {
        public UserIdentity()
        {
            UserName = "guest";
            Claims = new List<string>();
            Destinations = new List<string>();
            Brands = new List<string>();

        }

        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public Guid ApiKey { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<string> Destinations { get; set; }
        public IEnumerable<string> Brands { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}