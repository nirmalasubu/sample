using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Jobs.JobRegistry.CloudAmqpSync;
using OnDemandTools.Jobs.JobRegistry.Deporter;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using OnDemandTools.Jobs.JobRegistry.TitleSync;
using OnDemandTools.Jobs.Models;
using System;
using System.Diagnostics;

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
        CloudAmqpSync cloudAmqpSync;

        public HangfireController(Serilog.ILogger logger,
            AppSettings appsettings, 
            Publisher pub, 
            Deporter dep, 
            TitleSync tsy, 
            IQueueService queueService,
            CloudAmqpSync cloudAmqpSync
            )
        {
            this.logger = logger;
            this.pub = pub;
            this.dep = dep;
            this.tsy = tsy;
            this.queueService = queueService;
            this.appsettings = appsettings;
            this.cloudAmqpSync = cloudAmqpSync;
        }

        /// <summary>
        /// To Register new recurring Job initially
        /// </summary>
        /// <returns>Status</returns>

        [HttpGet]
        public string Get()
        {
            try
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appsettings.JobSchedules.TimeZone);

                var manager = new RecurringJobManager();

                manager.AddOrUpdate("Deporter", Job.FromExpression(() => dep.Execute()), 
                    appsettings.JobSchedules.Deporter, estTimeZone, HangfireQueue.deporter.ToString());

                manager.AddOrUpdate("TitleSync", Job.FromExpression(() => tsy.Execute()), 
                    appsettings.JobSchedules.TitleSync, estTimeZone, HangfireQueue.titlesync.ToString());

                manager.AddOrUpdate("CloudAmqpSync", Job.FromExpression(() => cloudAmqpSync.Execute()),
                    appsettings.JobSchedules.CloudAmqpSync, estTimeZone, HangfireQueue.titlesync.ToString());                

                foreach (var activeQueue in queueService.GetByStatus(true))
                {
                    if (activeQueue.Name.ToLower().StartsWith("unittest") && activeQueue.ContactEmailAddress.ToLower().Equals("ondemandtoolssupport@turner.com"))
                    {
                        manager.AddOrUpdate(string.Format("Publisher-{0}", activeQueue.Name),
                            Job.FromExpression(() => pub.Execute(activeQueue.Name)), appsettings.JobSchedules.Deporter, estTimeZone, HangfireQueue.publisher.ToString());
                    }
                    else
                    {
                        manager.AddOrUpdate(string.Format("Publisher-{0}", activeQueue.Name),
                            Job.FromExpression(() => pub.Execute(activeQueue.Name)), appsettings.JobSchedules.Publisher, estTimeZone, HangfireQueue.publisher.ToString());
                    }
                }

                return "Successfully registered jobs";
            }
            catch (Exception e)
            {
                logger.Error(e, "Error while registering Jobs");
                throw e;
            }

        }


        /// <summary>
        /// To Add new queue to the Publisher
        /// </summary>
        /// <param name="id">the queue name</param>
        [HttpPost("{id}")]
        public void Post(string id)
        {
            try
            {
                var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appsettings.JobSchedules.TimeZone);

                var manager = new RecurringJobManager();

                // Create multiple job among multiple instances
                manager.AddOrUpdate(string.Format("Publisher-{0}", id),
                    Job.FromExpression(() => pub.Execute(id)), appsettings.JobSchedules.Publisher, estTimeZone, HangfireQueue.publisher.ToString());

            }
            catch (Exception e)
            {
                logger.Error(e, "Error while registering Job");
                throw e;
            }
        }

        /// <summary>
        /// To remove a  queue in the publisher
        /// </summary>
        /// <param name="id">the queue name</param>
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            try
            {
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
