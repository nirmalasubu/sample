using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Routes
{
    public class Index: NancyModule       
    {
        public Index() : base("v1")
        {
            Get("/", _ =>
            {
                return Response.AsFile("Content/index.html", "text/html");
            });
        }
    }
}
