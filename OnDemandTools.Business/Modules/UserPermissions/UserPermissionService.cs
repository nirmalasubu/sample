using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using DLModel = OnDemandTools.DAL.Modules.UserPermissions.Model;
using OnDemandTools.DAL.Modules.UserPermissions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.UserPermissions.Command;
using OnDemandTools.DAL.Modules.UserPermissions.Queries;

namespace OnDemandTools.Business.Modules.UserPermissions
{
    public class UserPermissionService : IUserPermissionService
    {
        IUserPermissionQuery _query;
        IUserPermissionCommand _command;

        public UserPermissionService(IUserPermissionQuery query, IUserPermissionCommand userPermissionCommand)
        {
            _query = query;
            _command = userPermissionCommand;
        }
        public List<BLModel.UserPermission> GetAll(UserType userType)
        {
            return _query.Get().Where(p=>p.UserType == userType).ToList().ToBusinessModel<List<DLModel.UserPermission>, List<BLModel.UserPermission>>();
        }

        public BLModel.UserPermission GetById(string id)
        {
            return _query.GetById(id).ToBusinessModel<DLModel.UserPermission, BLModel.UserPermission>();
        }

        public BLModel.UserPermission Save(BLModel.UserPermission userPermission)
        {
            var model = _command.Save(userPermission.ToDataModel<BLModel.UserPermission, DLModel.UserPermission>());

            return model.ToBusinessModel<DLModel.UserPermission, BLModel.UserPermission>();
        }

        public List<BLModel.PortalModule> GetAllPortalModules()
        {
            return _query.GetAllPortalModules().OrderBy(p => p.DisplayOrder)
                .ToList().ToBusinessModel<List<DLModel.PortalModule>, List<BLModel.PortalModule>>();
        }

        public IList<BLModel.UserPermission> GetContactForByUserId(string id)
        {
            return _query.GetContactForByUserId(id).ToList<DLModel.UserPermission>().ToBusinessModel<List<DLModel.UserPermission>, List<BLModel.UserPermission>>();
        }
    }
}
