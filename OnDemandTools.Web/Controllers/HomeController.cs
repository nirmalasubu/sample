using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Package;
using Microsoft.AspNetCore.Http;

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
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated && User.HasClaim(c=>c.Value=="Read"))
            {
                return View();
            }
            else
            {
                if(HttpContext.User.Identity.IsAuthenticated)
                {
                    HttpContext.Session.SetString("NotAuthorized", "True");
                    return Redirect("Account/Snapout");
                }

                return View("Login");
            }

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
