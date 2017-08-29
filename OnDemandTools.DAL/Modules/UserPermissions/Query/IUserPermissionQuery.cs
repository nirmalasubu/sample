using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.UserPermissions.Query
{
    public interface IUserPermissionQuery
    {
        IQueryable<Model.UserPermission> Get();

        IQueryable<Model.PortalModule> GetAllPortalModules();

        Model.UserPermission GetById(string objectId);
    }
}
