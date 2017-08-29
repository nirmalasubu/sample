
using OnDemandTools.Common.Configuration;
using OnDemandTools.Web.Models.UserPermissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Config
{
    public class ConfigModel
    {
        public List<string> Brands { get; set; }
        public List<PortalModule> PortalModules { get; set; }
        public PortalSettings PortalSettings { get; set; }
    }
}
