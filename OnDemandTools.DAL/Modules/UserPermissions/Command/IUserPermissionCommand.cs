using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.UserPermissions.Command
{
    public interface IUserPermissionCommand
    {
        Model.UserPermission Save(Model.UserPermission model);
    }
}
