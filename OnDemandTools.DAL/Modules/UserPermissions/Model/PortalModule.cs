using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.UserPermissions.Model
{
    public class PortalModule
    {
        public string ModuleName { get; set; }

        public string ModuleDisplayName { get; set; }

        public string ModuleType { get; set; }

        public float DisplayOrder { get; set; }
    }
}
