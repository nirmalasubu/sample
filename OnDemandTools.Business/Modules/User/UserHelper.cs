using System;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.User.Model;
using DLModel = OnDemandTools.DAL.Modules.User.Model;
using OnDemandTools.DAL.Modules.User.Queries;
using System.Collections.Generic;
using OnDemandTools.Common.Model;
using System.Security.Claims;
using System.Security.Principal;

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
            userQuery.GetUsers().ToList();
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetBy(Guid apiKey)
        {
            BLModel.UserIdentity user = apiUserQuery.GetBy(apiKey).ToBusinessModel<DLModel.UserIdentity, BLModel.UserIdentity>();
            //user.AddClaim(new Claim("get", "get"));
            ClaimsPrincipal c = new ClaimsPrincipal(user);

            //BLModel.kk gc = new BLModel.kk();
            //gc.AddClaim(new Claim("get", "get"));
            //ClaimsPrincipal ccc = new ClaimsPrincipal(gc);

            //return ccc;
            return c;
        }

        public BLModel.UserIdentity GetById(string id)
        {
            userQuery.GetBy(id);
            return null;
        }

        public BLModel.UserIdentity GetByUserName(string userName)
        {
            userQuery.GetBy(userName).ToBusinessModel<DLModel.UserIdentity, BLModel.UserIdentity>();
            return null;
        }

    }
}
