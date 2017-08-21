using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.UserPermissions.Model
{
    public class Portal
    {
        public Portal()
        {
            Modules = new Dictionary<string, Permission>();
            DeliveryQueue = new Dictionary<string, Permission>();
        }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastLoginTime { get; set; }

        public Dictionary<string, Permission> Modules { get; set; }

        public Dictionary<string, Permission> DeliveryQueue { get; set; }
    }
}
