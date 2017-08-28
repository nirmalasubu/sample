using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.UserPermissions.Model
{
    [BsonIgnoreExtraElements]
    public class Portal
    {
        public Portal()
        {
            ModulePermissions = new Dictionary<string, Permission>();
            DeliveryQueuePermissions = new Dictionary<string, Permission>();
        }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastLoginTime { get; set; }

        public Dictionary<string, Permission> ModulePermissions { get; set; }

        public Dictionary<string, Permission> DeliveryQueuePermissions { get; set; }
    }
}
