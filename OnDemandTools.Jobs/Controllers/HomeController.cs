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

        //public IActionResult Index()
        //{
        //    return Redirect("/dashboard");
        //}


        Publisher pub;
        Deporter dep;
        TitleSync tsy;
        public HomeController(Publisher pub, Deporter dep, TitleSync tsy)
        {
            this.pub = pub;
            this.dep = dep;
            this.tsy = tsy;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("")]
        public IActionResult Register()
        {
            var manager = new RecurringJobManager();
            var id = Guid.NewGuid().ToString();
            manager.AddOrUpdate("Deporter-" + id, Job.FromExpression(() => dep.Execute()), "*/2 * * * *", TimeZoneInfo.Utc);
            manager.AddOrUpdate("Publisher-" + id, Job.FromExpression(() => pub.Execute()), "*/3 * * * *", TimeZoneInfo.Utc);
            manager.AddOrUpdate("TitleSync-" + id, Job.FromExpression(() => tsy.Execute()), "*/1 * * * *", TimeZoneInfo.Utc);

            return Redirect("/dashboard");
        }
    }
}
