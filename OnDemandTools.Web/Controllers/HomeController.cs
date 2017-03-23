using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnDemandTools.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View("Login");
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
