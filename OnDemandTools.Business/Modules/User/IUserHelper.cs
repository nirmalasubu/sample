using OnDemandTools.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OnDemandTools.Business.Modules.User
{
    public interface IUserHelper
    {
        List<UserIdentity> GetUsers();
        UserIdentity GetById(String id);
        ClaimsPrincipal GetByUserName(string userName);
        ClaimsPrincipal GetBy(Guid apiKey);
    }
}
