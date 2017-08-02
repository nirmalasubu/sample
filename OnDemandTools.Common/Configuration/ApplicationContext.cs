using Microsoft.AspNetCore.Http;

namespace OnDemandTools.Common.Configuration
{
    public interface IApplicationContext
    {
        UserIdentity GetUser();
        string GetUserName();
    }

    public class HttpAPIContext : IApplicationContext
    {
        IHttpContextAccessor cntx;
        

       public HttpAPIContext(IHttpContextAccessor cntx)
        {
            this.cntx = cntx;
        }

        public UserIdentity GetUser()
        {
            return cntx.HttpContext.User.Identity as UserIdentity;
        }

        public string GetUserName()
        {
            return cntx.HttpContext.User.Identity.Name;
        }
    }
}
