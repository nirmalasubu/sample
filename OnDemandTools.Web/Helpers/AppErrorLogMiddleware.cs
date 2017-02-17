using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace OnDemandTools.Web.Helpers
{
    public class AppErrorLogMiddleware
    {
        private readonly RequestDelegate _next;
        Serilog.ILogger logger;

        public AppErrorLogMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                logger.Error(e, string.Format("Error in Job Application: {0}", e));
                System.Diagnostics.Debug.WriteLine($"The following error happened: {e.Message}");
                throw e;
            }
        }
    }


    public static class AppErrorLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppErrorLogMiddleware>();
        }
    }
}
