using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization.Attributes;
using OnDemandTools.Common.Model;

namespace OnDemandTools.DAL.Modules.UserPermissions.Model
{
    [BsonIgnoreExtraElements]
    public class UserPermission : IModel
    {
        public UserPermission()
        {
            UserName = "";
        }

        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }
        public string Notes { get; set; }

        public UserType UserType { get; set; }
        public Api Api { get; set; }
        public Portal Portal { get; set; }

        public DateTime ActiveDateTime { get; set; }
       
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
