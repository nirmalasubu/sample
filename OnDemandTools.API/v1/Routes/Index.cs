using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Routes
{
    public class Index: NancyModule       
    {
        public Index()
        {
            Get("/", _ =>
            {
                return Response.AsFile("Content/index.html", "text/html");
            });

            Get("/healthcheck", _ =>
            {
                return Response.AsJson("Healthy", HttpStatusCode.OK);
            });
        }
    }
}
