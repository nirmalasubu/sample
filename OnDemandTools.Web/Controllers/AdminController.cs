using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnDemandTools.Web.Controllers
{
    
    public class AdminController : Controller
    {
        [Authorize("Admin")]
         public IActionResult Go()
        {           
            return View();
        }
    }
}
