using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Routes
{
    public class Index : NancyModule
    {
        public Index()
        {
            

            Get("/healthcheck", _ =>
            {
                return Response.AsJson("Healthy", HttpStatusCode.OK);
            });

            Get("/", _ =>
            {
                return View["Content/layout.html"];
            });

            Get("/(?:.*)", _ =>
            {
                return View["Content/layout.html"];
            });


            Get("/(?:.*)/(?:.*)", _ =>
            {
                return View["Content/layout.html"];
            });

            Get("/os", x =>
            {
                return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            });
        }
    }
}
