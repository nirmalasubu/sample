using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.UserPermissions.Model
{
    public class Portal
    {
        public Portal()
        {
            Modules = new Dictionary<string, Permission>();
            DeliveryQueue = new Dictionary<ObjectId, Permission>();
        }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastLoginTime { get; set; }

        public Dictionary<string, Permission> Modules { get; set; }

        public Dictionary<ObjectId, Permission> DeliveryQueue { get; set; }
    }
}
