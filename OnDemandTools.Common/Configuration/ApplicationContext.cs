using Microsoft.AspNetCore.Http;

namespace OnDemandTools.Common.Configuration
{
    public interface IApplicationContext
    {
        UserIdentity GetUser();
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
    }
}
