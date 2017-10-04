using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.UserPermissions
{
    public interface IMigrateUsersAndSystem 
    {
        List<string> Migrate();
    }
}
