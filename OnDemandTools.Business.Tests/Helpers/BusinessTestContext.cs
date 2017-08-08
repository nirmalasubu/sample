using System;
using System.Security.Principal;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Tests.Helpers
{
    public class BusinessTestContext : IApplicationContext
    {
        public IIdentity GetHttpIdentity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pass in actual user details to implement proper Authentication/Authorization
        /// </summary>
        /// <returns></returns>
        public UserIdentity GetUser()
        {
            return new UserIdentity { UserName = "BusinessUnitTest" };
        }

        /// <summary>
        /// Pass in actual user details to implement proper Authentication/Authorization
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return "BusinessUnitTest";
        }
    }
}