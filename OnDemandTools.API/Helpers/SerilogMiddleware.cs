using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Logzio;
using Serilog;
using Serilog.Events;

namespace OnDemandTools.API.Helpers
{

class InstrumentationMiddleware
    {
        static Serilog.ILogger log;

        readonly RequestDelegate _next;

        AppSettings appSettings;

        public InstrumentationMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
            this.appSettings = appSettings.Value;
             ConfigureLogzIOSettings();
        }

        private void ConfigureLogzIOSettings()
        {
            log = new LoggerConfiguration()
                      .WriteTo.Logzio(appSettings.LogzIO.AuthToken,
                      application: appSettings.LogzIO.Application,
                      reporterType: appSettings.LogzIO.ReporterType,
                      environment: appSettings.LogzIO.Environment)
                      .CreateLogger();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            Dictionary<string, object> info = new Dictionary<string, object>();

            var start = Stopwatch.GetTimestamp();
            try
            {
                await _next(httpContext);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                if(httpContext.Request.Path.HasValue && httpContext.Request.Path.Value.Contains("/v1/"))
                {
                    LogInformation(httpContext, elapsedMs, info, statusCode); 
                }                              
            }           
            catch (Exception ex) when (
                LogException(httpContext, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex, info)
            ) { }
        }

        bool LogException(HttpContext httpContext, double elapsedMs, Exception ex, Dictionary<string, object> info)
        {
            if(httpContext.Request.Headers.Any(c=>c.Key == "Authorization"))
            {
                 string api = httpContext.Request.Headers.FirstOrDefault(c=>c.Key == "Authorization").Value;
                 info.Add("api",api.Substring(api.Length - 4));
            }

            info.Add("context","Instrumentation");
            info.Add("requestMethod",httpContext.Request.Method);
            info.Add("path",httpContext.Request.Path.Value);
            info.Add("elapsedMs",elapsedMs);
            info.Add("status",500);
            info.Add("error", ex);
            log.Information("Request -"+httpContext.Request.Path+"{@info}", info); 

            return false;
        }


        void LogInformation(HttpContext httpContext, double elapsedMs, Dictionary<string, object> info, int? statusCode)
        {
            if(httpContext.Request.Headers.Any(c=>c.Key == "Authorization"))
            {
                string api = httpContext.Request.Headers.FirstOrDefault(c=>c.Key == "Authorization").Value;
                 info.Add("api",api.Substring(api.Length - 4));
            }

            info.Add("context","Instrumentation");
            info.Add("requestMethod",httpContext.Request.Method);
            info.Add("path",httpContext.Request.Path.Value);
            info.Add("elapsedMs",elapsedMs);
            info.Add("status", statusCode.Value);           
            log.Information("Request -"+httpContext.Request.Path+"{@info}", info); 

        }

              static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}