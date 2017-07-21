using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.Destination;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Distributed;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class CacheDemoController : Controller
    {
        AppSettings appsettings;
        private readonly IDistributedCache _distributedCache;

        public CacheDemoController(IDestinationService _destinationSvc, AppSettings appsettings, IDistributedCache distributedCache)
        {
            this.appsettings = appsettings;
            _distributedCache = distributedCache;
        }


        [HttpPost]
        public void AddToCache()
        {
            // if key already exist, this wll replace existing values
            _distributedCache.SetString("jjohn",DateTime.Now.ToString());
        }


        [HttpGet]
        public string GetCache()
        {
            return _distributedCache.GetString("jjohn");
        }


        [HttpDelete]
        public void DeleteCache()
        {
             _distributedCache.Remove("jjohn");
        }

        [HttpPut]
        public void UpdateCache()
        {            
             _distributedCache.SetString("jjohn", DateTime.Now.ToString());

        }
    }
}
