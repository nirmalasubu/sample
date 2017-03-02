using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Jobs.Helpers
{
    public class JobContext : IApplicationContext
    {
        /// <summary>
        /// Pass in actual user details to implement proper Authentication/Authorization
        /// </summary>
        /// <returns></returns>
        public UserIdentity GetUser()
        {
            return new UserIdentity { UserName = "JobsTestUser" };
        }
    }
}