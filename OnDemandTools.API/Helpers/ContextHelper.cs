using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using OnDemandTools.Business.Modules.User.Model;
using System.Net.Http;

namespace OnDemandTools.API.Helpers
{
    public static class ContextHelper
    {

        public static UserIdentity User(this NancyContext cs)
        {
            return (cs.Items["user"] as UserIdentity);
        }

        public static String Verb(this HttpMethod vb)
        {
            return vb.ToString().ToLower();
        }
    }
}
