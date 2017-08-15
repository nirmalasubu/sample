using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.UserPermissions.Model
{
    public class Permission
    {
        public Permission()
        {

        }

        public bool CanRead { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set;  }

        public bool CanDelete { get; set; }
    }
}
