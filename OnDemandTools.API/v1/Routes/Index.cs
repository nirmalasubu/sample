using Microsoft.Extensions.Configuration;
using Nancy;
using Newtonsoft.Json.Linq;
using OnDemandTools.Common.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy.Responses;
using System.IO;

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

                    if(provider.Count >= 1)
                    {
                        JObject hosting = new JObject();
                        hosting.Add("DeployedVersion", provider.SelectToken("containers[0].image").ToString().Split(':')[1]);
                        hosting.Add("Environment", provider.SelectToken("name").ToString());
                        hosting.Add("NumberOfInstancesRunning", provider.SelectToken("providers[0].replicas"));
                        hosting.Add("OperatingSystem", System.Runtime.InteropServices.RuntimeInformation.OSDescription);
                        jo.Add("HostingDetails", hosting);
                    }                   

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


             Get("/loaderio-29b8309d28125600c3242fe032077dc3", x =>
            {
                var file = new FileStream("loaderio-29b8309d28125600c3242fe032077dc3.txt", FileMode.Open);
                string fileName = "loaderio-29b8309d28125600c3242fe032077dc3.txt";

                var response = new StreamResponse(() => file, MimeTypes.GetMimeType(fileName));
                return response.AsAttachment(fileName);
            });
        }


        private Task<String> GetHostingProviderDetails(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<String>();
            theClient.ExecuteAsync(theRequest, response => {
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tcs.SetResult(response.Content);
                }
                else
                {
                    tcs.SetResult(String.Empty);
                }
                
            });
            return tcs.Task;
        }
    }
}
