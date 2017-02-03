using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using Hangfire.Common;
using System.Diagnostics;
using OnDemandTools.Jobs.JobRegistry.TitleSync;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using OnDemandTools.Jobs.JobRegistry.Deporter;

namespace OnDemandTools.Jobs.Controllers
{
   
    public class HomeController : Controller
    {

        Publisher pub;
        Deporter dep;
        TitleSync tsy;
        public HomeController(Publisher pub, Deporter dep, TitleSync tsy)
        {
            this.pub = pub;
            this.dep = dep;
            this.tsy = tsy;
        }


        [Route("")]
        public IActionResult Index()
        {
            return Redirect("/dashboard");
        }

        public IActionResult Whoami()
        {
            //TODO - add code similar to the API whoami

            return View();
        }

        [Route("/healthcheck")]
        public JsonResult Healthcheck()
        {
            return Json("Healthy");
        }

        public IActionResult Heartbeat()
        {
            //TODO - add code similar to job healthcheck that we have in ODT web

            return View();
        }



        public IActionResult Register()
        {
            //TODO - left here as reference. update as needed
            var manager = new RecurringJobManager();

            // Create multiple job among multiple instances
            manager.AddOrUpdate("Publisher-" + Guid.NewGuid().ToString(), Job.FromExpression(() => pub.Execute()), "*/3 * * * *", TimeZoneInfo.Utc);

            // Just oone job among multiple instances
            manager.AddOrUpdate("Deporter-", Job.FromExpression(() => dep.Execute()), "*/2 * * * *", TimeZoneInfo.Utc);
            manager.AddOrUpdate("TitleSync", Job.FromExpression(() => tsy.Execute()), "*/1 * * * *", TimeZoneInfo.Utc);

            return Redirect("/dashboard");
        }
    }
}
