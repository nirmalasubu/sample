using FBDService;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        [Route("/whoami")]
        public IActionResult Whoami()
        {
            // Construct application details like version, hosting environment, dependent services etc
            IDictionary<string, Dictionary<string, string>> details = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> appDetails = new Dictionary<string, string>();
            appDetails.Add("Name", appsettings.Name);
            appDetails.Add("Description", appsettings.Description);
            details.Add("ApplicationDetails", appDetails);
            RestClient client = new RestClient(appsettings.HostingProvider);
            var request = new RestRequest(Method.GET);
            Task.Run(async () =>
            {
                var rs = await GetHostingProviderDetails(client, request) as String;
                JObject provider = JObject.Parse(rs);

                if (provider.Count >= 1)
                {
                    Dictionary<string, string> hosting = new Dictionary<string, string>();
                    hosting.Add("DeployedVersion", provider.SelectToken("containers[0].image").ToString().Split(':')[1]);
                    hosting.Add("Environment", provider.SelectToken("name").ToString());
                    hosting.Add("NumberOfInstancesRunning", provider.SelectToken("providers[0].replicas").ToString());
                    hosting.Add("OperatingSystem", System.Runtime.InteropServices.RuntimeInformation.OSDescription);
                    details.Add("HostingDetails", hosting);
                }
            }).Wait();

            Dictionary<string, string> serviceProviders = new Dictionary<string, string>();
            foreach (var svc in appsettings.Services)
            {
                serviceProviders.Add(svc.Name, svc.Url);
            }
            details.Add("ExternalServiceProviders", serviceProviders);

            return Json(details);
        }

        [Route("/healthcheck")]
        public JsonResult Healthcheck()
        {

            return Json("Healthy");
        }

        [Route("/hangfireservers")]
        public JsonResult GetHangfireServers()
        {
            var servers = JobStorage.Current.GetMonitoringApi().Servers();

            return Json(servers);
        }

        [Route("/heartbeat")]
        public IActionResult Heartbeat()
        {
            Healthcheck hCheck = new Healthcheck();
            hCheck.IsAppHealthy = true;
            var con = JobStorage.Current.GetConnection();
            var servers = JobStorage.Current.GetMonitoringApi().Servers();

            var dateTimeExpire = DateTime.UtcNow.AddMinutes(-int.Parse(appsettings.JobSchedules.HeartBeatExpireMinute));
            if (servers.All(x => x.Heartbeat.HasValue && x.Heartbeat < dateTimeExpire))
            {
                hCheck.IsAppHealthy = false;  //server stop alert
            }
            // deporter Job
            var DeporterJob = con.GetAllEntriesFromHash($"recurring-job:{"Deporter"}");
            hCheck.DeporterAgentsHealth = GetJobState(con, DeporterJob);

            // Queues Job
            List<string> QueueJobsStatus = new List<string>();
            var activeQueues = queueService.GetByStatus(true);
            foreach (var activeQueue in activeQueues)
            {
                var QueueJob = con.GetAllEntriesFromHash($"recurring-job:{string.Format("Publisher-{0}", activeQueue.Name)}");
                QueueJobsStatus.Add(GetJobState(con, QueueJob));
            }
            if (QueueJobsStatus.All(x => x == "Recurring job not started"))
            {
                hCheck.PublisherAgentsHealth = "Recurring job not started";
            }
            else
            {
                int succeedQueueJob = QueueJobsStatus.Where(x => x == "excellent").Count();
                double postOfficePCT = ((Double)succeedQueueJob / activeQueues.Count()) * 100;
                if (postOfficePCT == 100.0) hCheck.PublisherAgentsHealth = "excellent";
                if (postOfficePCT < 100.0 && postOfficePCT >= 50.0) hCheck.PublisherAgentsHealth = "moderate";
                if (postOfficePCT < 50.0) hCheck.PublisherAgentsHealth = "critical";
            }

            // TitleSync Job
            var TitleSyncJob = con.GetAllEntriesFromHash($"recurring-job:{"TitleSync"}");
            hCheck.TitleSyncAgentsHealth = GetJobState(con, TitleSyncJob);

            return Json(hCheck);

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
            string state = "Recurring job not started";  // if LastJobId is not present then recurring job is not started
            if (Job != null && Job.ContainsKey("LastJobId"))
            {
                var d = con.GetJobData(Job["LastJobId"]);
                state = d.State == "Succeeded" ? "excellent" : "critical";
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
