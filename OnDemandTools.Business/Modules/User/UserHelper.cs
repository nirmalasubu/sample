using System;
using System.Linq;
using BLModel = OnDemandTools.Common.Configuration;
using DLModel = OnDemandTools.DAL.Modules.User.Model;
using OnDemandTools.DAL.Modules.User.Queries;
using System.Collections.Generic;
using OnDemandTools.Common.Model;
using System.Security.Claims;

namespace OnDemandTools.Business.Modules.User
{
    public class UserHelper : IUserHelper
    {
        IGetUsersQuery userQuery;
        IApiGetUserQuery apiUserQuery;

        public UserHelper(IGetUsersQuery userQuery, IApiGetUserQuery apiUserQuery)
        {
            this.userQuery = userQuery;
            this.apiUserQuery = apiUserQuery;
        }


        public List<BLModel.UserIdentity> GetUsers()
        {
           return userQuery.GetUsers().ToList().ToBusinessModel<List<DLModel.UserIdentity>, List<BLModel.UserIdentity>>();
        }

        public ClaimsPrincipal GetBy(Guid apiKey)
        {
            BLModel.UserIdentity user = apiUserQuery.GetBy(apiKey).ToBusinessModel<DLModel.UserIdentity, BLModel.UserIdentity>();
            ClaimsPrincipal userClaim = new ClaimsPrincipal(user);
            return (userClaim);
        }

        public BLModel.UserIdentity GetById(string id)
        {
            userQuery.GetBy(id);
            return null;
        }

        public ClaimsPrincipal GetByUserName(string userName)
        {
            BLModel.UserIdentity user = userQuery.GetBy(userName).ToBusinessModel<DLModel.UserIdentity, BLModel.UserIdentity>();
            ClaimsPrincipal userClaim = new ClaimsPrincipal(user);
            return (userClaim);
        }

    }
}
