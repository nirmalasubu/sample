using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.UserPermissions
{
    public interface IUserPermissionService
    {
        /// <summary>
        /// Gets all Userpermission
        /// </summary>
        /// <returns></returns>
        List<Model.UserPermission> GetAll(UserType userType);
    }
}
