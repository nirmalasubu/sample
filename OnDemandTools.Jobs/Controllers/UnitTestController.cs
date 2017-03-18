using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Jobs.JobRegistry.Publisher;
using System.Threading;
using System.Threading.Tasks;
using OnDemandTools.Jobs.JobRegistry.Mailbox;

namespace OnDemandTools.Jobs.Controllers
{

    [Route("api/[controller]")]
    public class UnitTestController : Controller
    {

        Publisher pub;
        Mailbox mail;

        public UnitTestController(Publisher pub, Mailbox mail)
        {
            this.mail = mail;
            this.pub = pub;
        }

        [HttpGet("{id}")]
        public  IActionResult ProcessPushlisher(string id)
        {           
            pub.Execute(id);
            return Json("Successfully processed");
        }
    }
}
