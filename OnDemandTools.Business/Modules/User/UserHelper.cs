using System;
using System.Linq;
using BLModel = OnDemandTools.Common.Configuration;
using System.Collections.Generic;
using OnDemandTools.Common.Model;
using System.Security.Claims;
using OnDemandTools.Business.Modules.UserPermissions.Model;
using OnDemandTools.Business.Modules.UserPermissions;

namespace OnDemandTools.Business.Modules.User
{
    public class UserHelper : IUserHelper
    {
        IUserPermissionService _userSvc;

        public UserHelper(IUserPermissionService userSvc)
        {
            _userSvc = userSvc;
        }

        public List<BLModel.UserIdentity> GetUsers()
        {
            List<UserPermission> users = _userSvc.GetAll(UserType.Portal).ToList();

            return users.ToBusinessModel<List<UserPermission>, List<BLModel.UserIdentity>>();
        }

        public ClaimsPrincipal GetBy(Guid apiKey)
        {
            BLModel.UserIdentity user = _userSvc.GetByApiKey(apiKey).ToBusinessModel<UserPermission, BLModel.UserIdentity>();
            ClaimsPrincipal userClaim = new ClaimsPrincipal(user);
            return (userClaim);
        }

        public BLModel.UserIdentity GetById(string id)
        {
            return _userSvc.GetById(id).ToBusinessModel<UserPermission, BLModel.UserIdentity>();
        }

        public ClaimsPrincipal GetByUserName(string userName)
        {
            BLModel.UserIdentity user = _userSvc.GetByUserName(userName).ToBusinessModel<UserPermission, BLModel.UserIdentity>();
            ClaimsPrincipal userClaim = new ClaimsPrincipal(user);
            return (userClaim);
        }

    }
}
