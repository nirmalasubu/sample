using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using DLModel = OnDemandTools.DAL.Modules.UserPermissions.Model;
using OnDemandTools.DAL.Modules.UserPermissions.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Common.Model;


namespace OnDemandTools.Business.Modules.UserPermissions
{
    public class UserPermissionService : IUserPermissionService
    {
        IUserPermissionQuery _query;

        public UserPermissionService(IUserPermissionQuery query)
        {
            _query = query;
        }
        public List<BLModel.UserPermission> GetAll(UserType userType)
        {
            return _query.Get().ToList().ToBusinessModel<List<DLModel.UserPermission>, List<BLModel.UserPermission>>();
        }
    }
}
