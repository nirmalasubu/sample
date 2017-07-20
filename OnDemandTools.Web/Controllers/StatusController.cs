using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Business.Modules.Status;
using OnDemandTools.Web.Models.Status;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Status.Model;
using OnDemandTools.Common.Model;
using System;

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

        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            statusService.Delete(id);
        }

        [Authorize]
        [HttpGet("newstatus")]
        public StatusModel GetEmptyModel()
        {
            return new StatusModel
            {
                Name = string.Empty,
                Description = string.Empty,
                User=string.Empty
            };
        }

        
        [Authorize]
        [HttpPost]
        public StatusModel Post([FromBody]StatusModel viewModel)
        {
            BLModel.Status blStatusModel = viewModel.ToBusinessModel<StatusModel, BLModel.Status>();

            if (string.IsNullOrEmpty(blStatusModel.Id))
            {
                blStatusModel.CreatedDateTime = DateTime.UtcNow;
                blStatusModel.CreatedBy = HttpContext.User.Identity.Name;
            }
            else
            {
                blStatusModel.ModifiedDateTime = DateTime.UtcNow;
                blStatusModel.ModifiedBy = HttpContext.User.Identity.Name;
            }

            blStatusModel = statusService.Save(blStatusModel);

            return blStatusModel.ToViewModel<BLModel.Status, StatusModel>();
        }
    }
}
