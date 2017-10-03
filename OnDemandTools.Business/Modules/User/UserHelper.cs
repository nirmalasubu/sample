using System;
using System.Linq;
using BLModel = OnDemandTools.Common.Configuration;
using System.Collections.Generic;
using OnDemandTools.Common.Model;
using System.Security.Claims;
using OnDemandTools.DAL.Modules.UserPermissions.Queries;
using OnDemandTools.DAL.Modules.UserPermissions.Model;

namespace OnDemandTools.Business.Modules.User
{
    public class UserHelper : IUserHelper
    {
        IUserPermissionQuery _userPermissionQuery;

        public UserHelper(IUserPermissionQuery userPermissionQuery)
        {
            _userPermissionQuery = userPermissionQuery;
        }

        public List<BLModel.UserIdentity> GetUsers()
        {
            List<UserPermission> users = _userPermissionQuery.Get().ToList();

            return users.ToBusinessModel<List<UserPermission>, List<BLModel.UserIdentity>>();
        }

        public ClaimsPrincipal GetBy(Guid apiKey)
        {
            BLModel.UserIdentity user = _userPermissionQuery.GetByApiKey(apiKey).ToBusinessModel<UserPermission, BLModel.UserIdentity>();
            ClaimsPrincipal userClaim = new ClaimsPrincipal(user);
            return (userClaim);
        }

        public BLModel.UserIdentity GetById(string id)
        {
            return _userPermissionQuery.GetById(id).ToBusinessModel<UserPermission, BLModel.UserIdentity>();
        }

        public ClaimsPrincipal GetByUserName(string userName)
        {
            BLModel.UserIdentity user = _userPermissionQuery.GetByUserName(userName).ToBusinessModel<UserPermission, BLModel.UserIdentity>();
            ClaimsPrincipal userClaim = new ClaimsPrincipal(user);
            return (userClaim);
        }

    }
}
