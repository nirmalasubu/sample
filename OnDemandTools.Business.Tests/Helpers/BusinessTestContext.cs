﻿using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Tests.Helpers
{
    public class BusinessTestContext : IApplicationContext
    {
        /// <summary>
        /// Pass in actual user details to implement proper Authentication/Authorization
        /// </summary>
        /// <returns></returns>
        public UserIdentity GetUser()
        {
            return new UserIdentity { UserName = "BusinessUnitTest" };
        }
    }
}