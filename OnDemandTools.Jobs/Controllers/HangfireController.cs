using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Jobs.JobRegistry.Deporter;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using OnDemandTools.Jobs.JobRegistry.TitleSync;
using OnDemandTools.Jobs.Models;
using System;

namespace OnDemandTools.Jobs.Controllers
{
    [Route("api/[controller]")]
    public class HangfireController : Controller
    {
        Serilog.ILogger logger;
        Publisher pub;
        Deporter dep;
        TitleSync tsy;
        IQueueService queueService;
        AppSettings appsettings;

        public HangfireController(Serilog.ILogger logger, AppSettings appsettings, Publisher pub, Deporter dep, TitleSync tsy, IQueueService queueService)
        {
            this.logger = logger;
            this.pub = pub;
            this.dep = dep;
            this.tsy = tsy;
            this.queueService = queueService;
            this.appsettings = appsettings;
        }

        [HttpGet]
        public string Get()
        {
            try
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appsettings.JobSchedules.TimeZone);

                var manager = new RecurringJobManager();


                manager.AddOrUpdate("Deporter", Job.FromExpression(() => dep.Execute()), appsettings.JobSchedules.Deporter, estTimeZone, HangfireQueue.deporter.ToString());
                manager.AddOrUpdate("TitleSync", Job.FromExpression(() => tsy.Execute()), appsettings.JobSchedules.TitleSync, estTimeZone, HangfireQueue.titlesync.ToString());

                foreach (var activeQueue in queueService.GetByStatus(true))
                {
                    // Create multiple job among multiple instances
                    manager.AddOrUpdate(string.Format("Publisher-{0}", activeQueue.Name),
                        Job.FromExpression(() => pub.Execute(activeQueue.Name)), appsettings.JobSchedules.Publisher, estTimeZone, HangfireQueue.pusblisher.ToString());
                }

                return "Successfully registered jobs";
            }
            catch (Exception e)
            {
                logger.Error(e, "Error while registering Jobs");
                throw e;
            }

        }

        [HttpPost("{id}")]
        public void Post(string id)
        {
            try
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appsettings.JobSchedules.TimeZone);

                var manager = new RecurringJobManager();

                // Create multiple job among multiple instances
                manager.AddOrUpdate(string.Format("Publisher-{0}", id),
                    Job.FromExpression(() => pub.Execute(id)), appsettings.JobSchedules.Publisher, estTimeZone, HangfireQueue.pusblisher.ToString());

            }
            catch (Exception e)
            {
                logger.Error(e, "Error while registering Job");
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            try
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appsettings.JobSchedules.TimeZone);

                var manager = new RecurringJobManager();

                manager.RemoveIfExists(string.Format("Publisher-{0}", id));

            }
            catch (Exception e)
            {
                logger.Error(e, "Error while removing the Job");
                throw e;
            }
        }
    }
}
