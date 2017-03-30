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

        public UnitTestController(Publisher pub, Mailbox mail)
        {
            this.pub = pub;
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
            UnitTestAiringCleanUp cleanUp = new UnitTestAiringCleanUp();
            var jobId = BackgroundJob.Enqueue(() => cleanUp.Execute(values));

            return Json("Successfully processed");
        }


    }
}
