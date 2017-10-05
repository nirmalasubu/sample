using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.User
{
    public class UserContactForAPI
    {

        public List<UserContactForAPIDetail> TechnicalContactFor { get; set; }

        public List<UserContactForAPIDetail> FunctionalContactFor { get; set; }
      
    }
}
