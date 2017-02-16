﻿using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Jobs.JobRegistry.Publisher;

namespace OnDemandTools.Jobs.Controllers
{

    [Route("api/[controller]")]
    public class UnitTestController : Controller
    {

        Publisher pub;

        public UnitTestController(Publisher pub)
        {
            this.pub = pub;
        }

        [HttpGet("{id}")]
        public IActionResult ProcessPushlisher(string id)
        {
            pub.Execute(id);

            return Json("Successfully processed");
        }
    }
}
