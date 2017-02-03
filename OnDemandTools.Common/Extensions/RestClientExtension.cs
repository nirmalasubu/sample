using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


public static class RestClientExtension
    {
        
        //TODO - add code to handle/propogate error properly
        public static Task<JObject> RetrieveRecord(this RestClient client, RestRequest request) 
        {
            var tcs = new TaskCompletionSource<JObject>();
            client.ExecuteAsync(request, response =>
            {                
                if(response.IsSuccessful())
                {
                    tcs.SetResult(JObject.Parse(response.Content));
                }
                else
                {
                    var jsonObject = new JObject();
                    jsonObject.Add("StatusCode", response.StatusCode.ToString());
                    jsonObject.Add("Error", response.ErrorMessage);
                    tcs.SetResult(jsonObject);
                }
            });

            return tcs.Task;
        }


        public static Task<JArray> RetrieveRecords(this RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<JArray>();
            client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful())
                {
                    tcs.SetResult(JArray.Parse(response.Content));
                }
                else
                {
                    JArray array = new JArray();
                    var jsonObject = new JObject();
                    jsonObject.Add("StatusCode", response.StatusCode.ToString());
                    jsonObject.Add("Error", response.ErrorMessage);
                    array.Add(jsonObject);
                    tcs.SetResult(array);
                }

                
            });

            return tcs.Task;
        }

        public static bool IsSuccessful(this IRestResponse response)
        {
            return response.StatusCode.IsSuccessStatusCode()
                && response.ResponseStatus == ResponseStatus.Completed;
        }

        public static bool IsSuccessStatusCode(this HttpStatusCode responseCode)
        {
            int numericResponse = (int)responseCode;
            return numericResponse >= 200
                && numericResponse <= 399;
        }
    }
namespace OnDemandTools.Common.Extensions
{
    public static class RestClientExtension
    {

        //TODO - add code to handle/propogate error properly
        public static Task<JObject> RetrieveRecord(this RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<JObject>();
            client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful())
                {
                    tcs.SetResult(JObject.Parse(response.Content));
                }
                else
                {
                    var jsonObject = new JObject();
                    jsonObject.Add("StatusCode", response.StatusCode.ToString());
                    jsonObject.Add("Error", response.ErrorMessage);
                    tcs.SetResult(jsonObject);
                }
            });

            return tcs.Task;
        }


        public static Task<JArray> RetrieveRecords(this RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<JArray>();
            client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful())
                {
                    tcs.SetResult(JArray.Parse(response.Content));
                }
                else
                {
                    JArray array = new JArray();
                    var jsonObject = new JObject();
                    jsonObject.Add("StatusCode", response.StatusCode.ToString());
                    jsonObject.Add("Error", response.ErrorMessage);
                    array.Add(jsonObject);
                    tcs.SetResult(array);
                }


            });

            return tcs.Task;
        }

        public static bool IsSuccessful(this IRestResponse response)
        {
            return response.StatusCode.IsSuccessStatusCode()
                && response.ResponseStatus == ResponseStatus.Completed;
        }

        public static bool IsSuccessStatusCode(this HttpStatusCode responseCode)
        {
            int numericResponse = (int)responseCode;
            return numericResponse >= 200
                && numericResponse <= 399;
        }
    }
}
