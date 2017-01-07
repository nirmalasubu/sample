using Microsoft.Extensions.Configuration;
using Nancy;
using Newtonsoft.Json.Linq;
using OnDemandTools.Common.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Routes
{
    public class Index : NancyModule
    {
        
        public Index(AppSettings configuration)
        {
            Get("/healthcheck", _ =>
            {
                return Response.AsJson("Healthy", HttpStatusCode.OK);
            });

            Get("/", _ =>
            {
                return View["Content/layout.html"];
            });

            Get("/(?:.*)", _ =>
            {
                return View["Content/layout.html"];
            });


            Get("/(?:.*)/(?:.*)", _ =>
            {
                return View["Content/layout.html"];
            });

            Get("/whoami", x =>
            {
                // Construct application details like version, hosting environment, dependent services etc
                JObject jo = new JObject();
                jo.Add("Name", configuration.Name);
                jo.Add("Description", configuration.Description);

                RestClient client = new RestClient(configuration.HostingProvider);
                var request = new RestRequest(Method.GET);
                Task.Run(async () =>
                {
                    var rs = await GetHostingProviderDetails(client, request) as String;
                    JObject provider = JObject.Parse(rs);
                    JObject hosting = new JObject();
                    hosting.Add("DeployedVersion", provider.SelectToken("containers[0].image").ToString().Split(':')[1]);
                    hosting.Add("DeployedEnvironment", provider.SelectToken("name").ToString());
                    hosting.Add("NumberOfInstancesRunning", provider.SelectToken("providers[0].replicas"));
                    hosting.Add("OperatingSystem", System.Runtime.InteropServices.RuntimeInformation.OSDescription);
                    jo.Add("HostingDetails", hosting);

                }).Wait();                

                JArray serviceProviders = new JArray();
                foreach (var svc in configuration.Services)
                {
                    JObject s = new JObject();                    
                    s.Add(svc.Name, svc.Url);
                    serviceProviders.Add(s);
                }
                jo.Add("ExternalServiceProviders", serviceProviders);


                // Return result
                return Response.AsJson(jo);
            });
        }


        private Task<String> GetHostingProviderDetails(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<String>();
            theClient.ExecuteAsync(theRequest, response => {
                var kk = response.Content;

                tcs.SetResult(response.Content);
            });
            return tcs.Task;
        }
    }
}
