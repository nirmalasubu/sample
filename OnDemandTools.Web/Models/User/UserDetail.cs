using OnDemandTools.Web.Models.UserPermissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.User
{
    public class UserDetail
    {
        public UserPermission UserPermission { get; set; }

        public UserContactForAPI UserContactForAPI { get; set; }
    }
}
