using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Business.Modules.Status;
using OnDemandTools.Web.Models.Status;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Status.Model;
using OnDemandTools.Common.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        IStatusSerivce statusService;
        AppSettings appSettings;

        public StatusController(AppSettings appSettings, IStatusSerivce statusService)
        {
            this.appSettings = appSettings;
            this.statusService = statusService;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public List<StatusModel> Get()
        {
            var statuses = statusService.GetAllStatus().ToList();

            return statuses.ToViewModel<List<BLModel.Status>, List<StatusModel>>();
        }
    }
}
