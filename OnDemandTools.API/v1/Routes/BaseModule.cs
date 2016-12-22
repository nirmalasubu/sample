using Microsoft.Extensions.Configuration;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Routes
{
    public class BaseModule : NancyModule
    {
       
        public IConfigurationRoot Configuration{ get; set; }

        public Serilog.ILogger Logger { get; set; }

        public BaseModule() 
        {

        }

        public BaseModule(String modulePath):base(modulePath)
        {

        }

        //TODO - find out why this cannot be accessed bin child classess
        public BaseModule(IConfigurationRoot config, Serilog.ILogger log)
        {
            Configuration = config;
            Logger = log;
        }
    }
}
