using System;
using System.Security.Principal;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Jobs.Tests.Helpers
{
    public class JobContext : IApplicationContext
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
            return new UserIdentity { UserName = "JobsUser" };
        }

        /// <summary>
        /// Pass in actual user details to implement proper Authentication/Authorization
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return "JobsUser";
        }
    }
}