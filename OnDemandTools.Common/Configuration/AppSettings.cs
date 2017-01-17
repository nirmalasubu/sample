using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Common.Configuration
{

    /// <summary>
    /// Binder class for service related information
    /// </summary>
    public class AppSettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MongoDB MongoDB { get; set;}
        public LogzIOConfiguration LogzIO { get; set; }
        public List<Service> Services { get; set; }
        public string HostingProvider { get; set; }
        public string AiringIdLockExpiredSeconds { get; set; }
    }


    public class Service
    {
        public String Name { get; set; }
        public String Url { get; set; }
        public String ApiKey { get; set; }
    }

    public class LogzIOConfiguration 
    {
        public string AuthToken { get; set; }
        public string Application { get; set; }
        public string ReporterType { get; set; }
        public string Environment { get; set; }
    }

    public class MongoDB
    {
        public string ConnectionString { get; set; }
        public string ConnectionOptionsDefault { get; set; }
        public string ConnectionOptionsPrimary { get; set; }
    }
}
