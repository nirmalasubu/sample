using OnDemandTools.Business.Modules.User.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OnDemandTools.Business.Modules.User
{
    public interface IUserHelper
    {
        List<UserIdentity> GetUsers();
        UserIdentity GetById(String id);
        UserIdentity GetByUserName(string userName);
        ClaimsPrincipal GetBy(Guid apiKey);
    }
}
