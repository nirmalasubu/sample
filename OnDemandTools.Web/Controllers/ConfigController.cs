using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Common.Configuration;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        AppSettings _appSettings;
        public ConfigController(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public PortalSettings Get()
        {
            return _appSettings.PortalSettings;
        }
    }
}
