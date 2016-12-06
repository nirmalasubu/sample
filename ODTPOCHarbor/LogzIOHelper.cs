using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ODTPOCHarbor
{
    public class LogzIOServiceHelper
    {
        HttpWebRequest request;
        byte[] requestBody;

        public void Send(Dictionary<string, object> message)
        {
            try
            {
                var sb = new StringBuilder();

                Dictionary<String, object> keys = new Dictionary<String, object>();
                keys.Add("type", "ODT_Docker_Monitoring");
                keys.Add("application", "ODT_Harbor");

                foreach (KeyValuePair<string, object> kvp in message)
                {
                    keys.Add(kvp.Key, kvp.Value);
                }


                sb.Append(ToJson(keys));
                requestBody = Encoding.UTF8.GetBytes(sb.ToString());

                var url = String.Format("{0}://{1}:{2}?token={3}",
                                        "http",
                                         "listener.logz.io",
                                         8090,
                                         "PKzpHRkmGSdahHHIpeprYuKixClLUTRh");
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers["User-Agent"] = "ODT_Harbor Logz";               
                request.ContentType = "plain/text";
                request.Headers["Content-Length"] = requestBody.Length.ToString();
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);



                


                //using (var requestStream = request.BeginGetRequestStream)
                //{
                //    requestStream.Write(requestBody, 0, requestBody.Length);
                //    requestStream.Flush();
                //    requestStream.Close();
                //}

                //using (var response = request.GetResponse())
                //{
                //    response.Close();

                //}
            }
            catch (Exception)
            {
                

            }

        }


        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            // Write to the request stream.
            postStream.Write(requestBody, 0, requestBody.Length);
            postStream.Dispose();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }


        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            //Stream streamResponse = response.GetResponseStream();
            //StreamReader streamRead = new StreamReader(streamResponse);
            //string responseString = streamRead.ReadToEnd();
            //Debug.WriteLine(responseString);
            //// Close the stream object
            //streamResponse.Dispose();
            //streamRead.Dispose();

            // Release the HttpWebResponse
            response.Dispose();        
        }


        private String ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

    }

}
