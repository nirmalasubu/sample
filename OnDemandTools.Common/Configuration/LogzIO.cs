using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Common.Configuration
{
    public interface ILoggerSettings
    {
        string AuthToken { get; set; }
        string Application { get; set; }
        string ReporterType { get; set; }
        string Environment { get; set; }
    }

    //public class LogzIOConfiguration:ILoggerSettings
    //{
    //    public string AuthToken { get; set; }
    //    public string Application { get; set; }
    //    public string ReporterType { get; set; }
    //    public string Environment { get; set; }
    //}

}
