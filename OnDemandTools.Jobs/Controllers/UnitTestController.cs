using System;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using OnDemandTools.Jobs.JobRegistry.Airings;
using OnDemandTools.Jobs.JobRegistry.Mailbox;

namespace OnDemandTools.Jobs.Controllers
{

    [Route("api/[controller]")]
    public class UnitTestController : Controller
    {
        Publisher pub;
        PurgeUnitTestAirings purgeAirings;

        public UnitTestController(Publisher pub, PurgeUnitTestAirings purgeAirings)
        {
            this.pub = pub;
            this.purgeAirings = purgeAirings;
        }

        [HttpGet("{id}")]
        public IActionResult ProcessPushlisher(string id)
        {
            pub.Execute(id);
            return Json("Successfully processed");
        }

        [HttpPost]
        public IActionResult CleanupAirings([FromBody]string[] values)
        {
            var jobId = BackgroundJob.Enqueue(() => purgeAirings.Execute(values));

            return Json("Successfully processed");
        }


    }
}
