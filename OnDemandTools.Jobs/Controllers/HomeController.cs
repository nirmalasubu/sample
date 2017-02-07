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
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Jobs.Controllers
{

    public class HomeController : Controller
    {
        Serilog.ILogger logger;
        Publisher pub;
        Deporter dep;
        TitleSync tsy;
        IQueueService queueService;
        AppSettings appsettings;

        public HomeController(Serilog.ILogger logger, AppSettings appsettings, Publisher pub, Deporter dep, TitleSync tsy, IQueueService queueService)
        {
            this.logger = logger;
            this.pub = pub;
            this.dep = dep;
            this.tsy = tsy;
            this.queueService = queueService;
            this.appsettings = appsettings;
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

        public IActionResult TimeZones()
        {
            string time = " ";
            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
            {
                time += zone.Id + "  " + zone.DisplayName + "\n";
            }

            logger.Information(time);

            return new ContentResult
            {
                ContentType = "application/text",
                Content = time,
                StatusCode = 200
            };
        }

        public IActionResult Register()
        {
            try
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                string time=" ";
                 foreach(TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
                {
                    time += zone.Id + "  " + zone.DisplayName+ "\n";
                   
                }
                logger.Information(time);
                //TODO - left here as reference. update as needed
                var manager = new RecurringJobManager();

                foreach (var activeQueue in queueService.GetByStatus(true))
                {
                    // Create multiple job among multiple instances
                    manager.AddOrUpdate(string.Format("Publisher-{0}", activeQueue.Name),
                        Job.FromExpression(() => pub.Execute(activeQueue.Name)), appsettings.JobSchedules.Publisher, estTimeZone);
                }

                // Just oone job among multiple instances
                manager.AddOrUpdate("Deporter", Job.FromExpression(() => dep.Execute()), appsettings.JobSchedules.Deporter, estTimeZone);
                manager.AddOrUpdate("TitleSync", Job.FromExpression(() => tsy.Execute()), appsettings.JobSchedules.TitleSync, estTimeZone);

                return Redirect("/dashboard");
            }
            catch (Exception e)
            {
                logger.Error(e, "Error while registering Jobs");
                throw e;
            }
        }
    }
}
