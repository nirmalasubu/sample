using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using OnDemandTools.Common.Extensions;

namespace OnDemandTools.Common.Logzio
{
    /// <summary>
    /// Custom Serilog sink for Logzio
    /// </summary>
    public sealed class LogzioSink : PeriodicBatchingSink
    {
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2.0);
        private IHttpClient client;
        private readonly string requestUri;
        private const string LogzIOUrl = "http://listener.logz.io:8070/?token={0}";
        private string application;
        private string reporterType;
        private string environment;

        public static int DefaultBatchPostingLimit { get; } = 1000;

        public LogzioSink(IHttpClient client, string authToken, int batchPostingLimit, TimeSpan period, IFormatProvider formatProvider, string application, string reporterType, string environment)
          : base(batchPostingLimit, period)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            if (authToken == null)
                throw new ArgumentNullException("authToken");
            this.client = client;
            this.application = application;
            this.reporterType = reporterType;
            this.environment = environment;
            this.requestUri = string.Format("http://listener.logz.io:8070/?token={0}", (object)authToken);
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            HttpResponseMessage httpResponseMessage = await this.client.PostAsync(this.requestUri, (HttpContent)new StringContent(this.FormatPayload(events), Encoding.UTF8, "application/json")).ConfigureAwait(false);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new LoggingFailedException(string.Format("Received failed result {0} when posting events to {1}", (object)httpResponseMessage.StatusCode, (object)this.requestUri));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing || this.client == null)
                return;
            this.client.Dispose();
            this.client = (IHttpClient)null;
        }

        private string FormatPayload(IEnumerable<LogEvent> events)
        {
            return string.Join(",\n", events.Select<LogEvent, string>(new Func<LogEvent, string>(this.FormatLogEvent)).ToArray<string>());
        }

        private string FormatLogEvent(LogEvent curEvent)
        {

            dynamic expando = new ExpandoObject();
            var p = expando as IDictionary<string,object>;
            p.Add("application", this.application);
            p.Add("type", this.reporterType);
            p.Add("environment", this.environment);
            p.Add("level", curEvent.Level.ToString());
            p.Add("message", curEvent.MessageTemplate.Tokens.FirstOrDefault().ToString());

            if (curEvent.Exception != null)
                p.Add("exception", curEvent.Exception.StackTrace);                

            if(curEvent.Properties.Any())
            {
                foreach (KeyValuePair<string, LogEventPropertyValue> kvp in curEvent.Properties)
                {
                    List<string> lst = kvp.Value.ToString().Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "")
                            .Split(',')
                            .Select(c=>c)
                            .ToList();

                    foreach (var item in lst)
                    {                        
                        Regex splitter = new Regex(@"^(.+""\s*):(\s*.+)$");
                        var items = splitter.Split(item).Where(s => s!= String.Empty);
                        if(!items.IsNullOrEmpty())
                        {
                            double nr;
                            if(Double.TryParse(items.ElementAt(1).Replace(@"""", "").Trim(), out nr))
                            {
                                p.Add(items.ElementAt(0).Replace(@"""", "").Trim(),nr);
                            }
                            else{
                                p.Add(items.ElementAt(0).Replace(@"""", "").Trim(), items.ElementAt(1).Replace(@"""", "").Trim());
                            }  
                        }
                      
                    }
                }
              
            }
            
            return (JsonConvert.SerializeObject((object)expando, Formatting.None));

        }
    }


    // Helpers for Logzio sink
    public static class LoggerSinkConfigurationExtensions
    {
        public static LoggerConfiguration Logzio(this LoggerSinkConfiguration sinkConfiguration, string authToken, String application, string reporterType, string environment, int? batchPostingLimit = null, TimeSpan? period = null, IFormatProvider formatProvider = null, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
        {
            if (sinkConfiguration == null)
                throw new ArgumentNullException("sinkConfiguration");
            LogzioSink logzioSink = new LogzioSink((IHttpClient)new HttpClientWrapper(), authToken, batchPostingLimit ?? LogzioSink.DefaultBatchPostingLimit, period ?? LogzioSink.DefaultPeriod, formatProvider,  application, reporterType, environment);
            return sinkConfiguration.Sink((ILogEventSink)logzioSink, restrictedToMinimumLevel);
        }
    }

    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

        void Dispose();
    }


    internal class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient client;

        public HttpClientWrapper()
        {
            this.client = new HttpClient();
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return this.client.PostAsync(requestUri, content);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }

}