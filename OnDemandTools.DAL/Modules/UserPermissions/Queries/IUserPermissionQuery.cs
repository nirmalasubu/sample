using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.UserPermissions.Queries
{
    public interface IUserPermissionQuery
    {
        IQueryable<Model.UserPermission> Get();

        IQueryable<Model.PortalModule> GetAllPortalModules();

        Model.UserPermission GetById(string objectId);

        Model.UserPermission GetByUserName(string emailAddress);

        IQueryable<Model.UserPermission> GetContactForByUserId(string id);
    }
}
