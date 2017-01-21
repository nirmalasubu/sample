using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

using Newtonsoft.Json.Linq;
using System.Net;

namespace OnDemandTools.Business.Tests.Helpers
{
    public class BusinessTestFixture : IDisposable
    {
        public IConfigurationRoot Configuration { get; }

        public BusinessTestFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }



        public void Dispose()
        {
            
        }
    }
}
