using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Package;

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
            return View();
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
