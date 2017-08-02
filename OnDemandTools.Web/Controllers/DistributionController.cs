using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Common.Model;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.Business.Modules.AiringId;
using OnDemandTools.Web.Models.Distribution;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class DistributionController : Controller
    {
        IAiringIdService airingSvc;
        AppSettings appsettings;

        public DistributionController(IAiringIdService airingSvc,
            AppSettings appsettings
            )
        {
            this.appsettings = appsettings;
            this.airingSvc = airingSvc;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public IEnumerable<CurrentAiringIdViewModel> Get()
        {
            List<CurrentAiringIdViewModel> currentAiringIdModel = airingSvc.GetAllCurrentAiringIds().ToViewModel<List<CurrentAiringId>, List<CurrentAiringIdViewModel>>();

            return currentAiringIdModel;
        }

        // GET: api/values
        [Authorize]
        [HttpPost("generate/{prefix}")]
        public CurrentAiringIdViewModel GenerateAiringId(string prefix)
        {
            CurrentAiringIdViewModel currentAiringIdModel = airingSvc.Distribute(prefix).ToViewModel<CurrentAiringId, CurrentAiringIdViewModel>();

            return currentAiringIdModel;
        }

        [Authorize]
        [HttpGet("newcurrentairingid")]
        public CurrentAiringIdViewModel GetEmptyModel()
        {
            //return new CurrentAiringIdViewModel
            //{
            //    Name = string.Empty,
            //    Description = string.Empty,
            //    ExternalId = string.Empty
            //};
            return null;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            airingSvc.DeleteById(id);
        }
    }
}
