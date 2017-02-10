using FBDService;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Queue;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Jobs.Models;
using OrionService;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Jobs.Controllers
{

    public class HomeController : Controller
    {
        Serilog.ILogger logger;       
        IQueueService queueService;
        AppSettings appsettings;

        public HomeController(Serilog.ILogger logger, AppSettings appsettings, IQueueService queueService)
        {
            this.logger = logger;
            this.queueService = queueService;
            this.appsettings = appsettings;
        }


        [Route("")]
        public IActionResult Index()
        {
            return Redirect("/dashboard");
        }

        [Route("/Whoami")]
        public IActionResult Whoami()
        {
            //TODO - add code similar to the API whoami

            return View();
        }

        [Route("/healthcheck")]
        public JsonResult Healthcheck()
        {

            Healthcheck hCheck = new Healthcheck();
            var con = JobStorage.Current.GetConnection();
            var servers = JobStorage.Current.GetMonitoringApi().Servers();
            var server = JobStorage.Current.GetComponents();
            var dateTimeExpire = DateTime.UtcNow.AddMinutes(-1);
            if (servers.All(x => x.Heartbeat.HasValue && x.Heartbeat < dateTimeExpire))
            {
                hCheck.IsAppHealthy = false;  //server stop alert
            }
            var DeporterJob = con.GetAllEntriesFromHash($"recurring-job:{"Deporter"}");
            hCheck.DeporterAgentsHealth = GetJobState(con, DeporterJob) == "Succeeded" ? "excellent" : "critical";

            List<string> QueueJobsStatus = new List<string>();
            var activeQueues = queueService.GetByStatus(true);
            foreach (var activeQueue in activeQueues)
            {
                var QueueJob = con.GetAllEntriesFromHash($"recurring-job:{string.Format("Publisher-{0}", activeQueue.Name)}");
                QueueJobsStatus.Add(GetJobState(con, QueueJob));
            }
            int succeedQueueJob = QueueJobsStatus.Where(x => x == "Succeeded").Count();
            double postOfficePCT = ((Double)succeedQueueJob / activeQueues.Count()) * 100;
            if (postOfficePCT == 100.0) hCheck.PublisherAgentsHealth = "excellent";
            if (postOfficePCT < 100.0 && postOfficePCT >= 50.0) hCheck.PublisherAgentsHealth = "moderate";
            if (postOfficePCT < 50.0) hCheck.PublisherAgentsHealth = "critical";

            var TitleSyncJob = con.GetAllEntriesFromHash($"recurring-job:{"TitleSync"}");
            hCheck.TitleSyncAgentsHealth = GetJobState(con, TitleSyncJob) == "Succeeded" ? "excellent" : "critical";

            return Json(hCheck);
        }

        public IActionResult Heartbeat()
        {
            //TODO - add code similar to job healthcheck that we have in ODT web

            return View();
        }

        [Route("/error")]
        public IActionResult Error()
        {
            //TODO - If required write a error view

            return Json("Error in the application");
        }

        public IActionResult CheckBim(string id)
        {
            var endpoint = new FBDWSSoapClient.EndpointConfiguration();

            GetBIMRecordByMaterialIdResponse response = null;

            var client = new FBDWSSoapClient(endpoint);

            Task.Run(async () =>
            {
                response = await client.GetBIMRecordByMaterialIdAsync(id + "%");
            }).Wait();

            return Json(response);
        }

        public IActionResult CheckOrion(string id)
        {
            var client = new InventoryClient();
            var request = new BasicVersionByCID
            {
                ContentID = new string[] { id }
            };

            GetBasicVersionInformationByCIDResponseMessage response = null;

            Task.Run(async () =>
            {
                response = await client.GetBasicVersionInformationByCIDAsync(request);
            }).Wait();

            return Json(response);
        }


        #region Private methods

        private string GetJobState(IStorageConnection con, Dictionary<string, string> Job)
        {
            string state = "Failed";
            if (Job != null && Job.ContainsKey("LastJobId"))
            {
                var d = con.GetJobData(Job["LastJobId"]);
                state = d.State;
            }
            return state;
        }


        private Task<string> GetHostingProviderDetails(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<String>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tcs.SetResult(response.Content);
                }
                else
                {
                    tcs.SetResult(string.Empty);
                }

            });
            return tcs.Task;
        }
        #endregion
    }
}
