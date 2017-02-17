using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnDemandTools.Web.Controllers
{
    [Route("/[controller]")]
    public class HomeController : Controller
    {

        [Route("/error")]
        public IActionResult Error()
        {
            return View("");
        }

        [Route("/")]
        public IActionResult Index()
        {
            return Json("hi");
        }

        [Route("/healthcheck")]
        public JsonResult Healthcheck()
        {
            return Json("Healthy");
        }
    }
}
