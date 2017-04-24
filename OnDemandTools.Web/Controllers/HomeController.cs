using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Package;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace OnDemandTools.Web.Controllers
{
    public class HomeController : Controller
    {
        IHttpContextAccessor httpContextAccessor;
        IPackageService svc;
        public HomeController(IHttpContextAccessor httpContextAccessor, IPackageService svc)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.svc = svc;
        }

        [Route("")]
        [Route("/home")]
        [Route("/deliveryQueues")]
        [Route("/products")]
        [Route("/destinations")]
        [Route("/permissions")]
        [Route("/pendingRequests")]
        [Route("/contentTiers")]
        [Route("/workflowStatuses")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated && User.HasClaim(c=>c.Value=="read"))
            {
                return View("Index");
            }
            else
            {
                if(HttpContext.Session.Keys.Contains("NotAuthorized"))
                    ViewData["NotAuthorized"] = HttpContext.Session.GetString("NotAuthorized");               
                    
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

    }
}
