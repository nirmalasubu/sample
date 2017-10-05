using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.User
{
    public class UserContactForAPIDetail
    {
        public string UserName { get; set; }

        public string ApiKey { get; set; }

        public bool IsActive { get; set; }
    }
}
