using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.UserPermissions
{
    public class UserContactFor
    {
        public List<string> TechnicalContactFor { get; set; }

        public List<string> FunctionalContactFor { get; set; }
    }
}
