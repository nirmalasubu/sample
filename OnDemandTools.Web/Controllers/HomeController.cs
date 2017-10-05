using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Package;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using RestSharp;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Web.Controllers
{
    public class HomeController : Controller
    {
        IHttpContextAccessor httpContextAccessor;
        IPackageService svc;
        AppSettings appsettings;
        public HomeController(IHttpContextAccessor httpContextAccessor, IPackageService svc,
             AppSettings appsettings)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.svc = svc;
            this.appsettings = appsettings;
        }

        [Route("")]
        [Route("/home")]
        [Route("/deliveryQueues")]
        [Route("/products")]
        [Route("/destinations")]
        [Route("/userManagement")]
        [Route("/systemManagement")]
        [Route("/pendingRequests")]
        [Route("/contentTiers")]
        [Route("/workflowStatuses")]
        [Route("/categories")]
        [Route("/airingIds")]
        [Route("/pathtranslations")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated && User.HasClaim(c => c.Value == "read"))
            {
                return View("Index");
            }
            else
            {
                if (HttpContext.Session.Keys.Contains("NotAuthorized"))
                    ViewData["NotAuthorized"] = HttpContext.Session.GetString("NotAuthorized");

                HttpContext.Session.SetString("RedirectURL", HttpContext.Request.Path);

                return View("Login");
            }
        }


        [Authorize(Policy = "READ")]
        // GET: /Home/Dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View("Index");

        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("/healthcheck")]
        public JsonResult Healthcheck()
        {
            return Json("Healthy");
        }

        [Route("/whoami")]
        public IActionResult Whoami()
        {
            // Construct application details like version, hosting environment, dependent services etc
            JObject jo = new JObject();
            jo.Add("Name", appsettings.Name);
            jo.Add("Description", appsettings.Description);


            RestClient client = new RestClient(appsettings.HostingProvider);
            var request = new RestRequest(Method.GET);
            Task.Run(async () =>
            {
                var rs = await GetHostingProviderDetails(client, request) as String;
                JObject provider = JObject.Parse(rs);

                if (provider.Count >= 1)
                {
                    JObject hosting = new JObject();
                    hosting.Add("DeployedVersion", provider.SelectToken("containers[0].image").ToString().Split(':')[1]);
                    hosting.Add("Environment", provider.SelectToken("name").ToString());
                    hosting.Add("NumberOfInstancesRunning", provider.SelectToken("providers[0].replicas"));
                    hosting.Add("OperatingSystem", System.Runtime.InteropServices.RuntimeInformation.OSDescription);
                    jo.Add("HostingDetails", hosting);
                }

            }).Wait();

            JArray serviceProviders = new JArray();
            foreach (var svc in appsettings.Services)
            {
                JObject s = new JObject();
                s.Add(svc.Name, svc.Url);
                serviceProviders.Add(s);
            }
            jo.Add("ExternalServiceProviders", serviceProviders);


            return Json(jo);
        }

        private Task<string> GetHostingProviderDetails(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<String>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tcs.SetResult(response.Content);
                }
                else
                {
                    tcs.SetResult(string.Empty);
                }

            });
            return tcs.Task;
        }

    }
}
