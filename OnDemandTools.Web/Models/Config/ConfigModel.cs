using OnDemandTools.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Config
{
    public class ConfigModel
    {
       public  List<string> Brands { get; set; }
        public PortalSettings PortalSettings { get; set; }
    }
}
