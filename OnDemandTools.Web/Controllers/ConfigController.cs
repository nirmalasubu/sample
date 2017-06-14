using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Business.Modules.Brands;
using OnDemandTools.Web.Models.Config;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        AppSettings _appSettings;
        IBrandService _brandService;

        public ConfigController(AppSettings appSettings, IBrandService brandService)
        {
            _appSettings = appSettings;
            _brandService = brandService;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public ConfigModel Get()
        {
            return new ConfigModel
            {
                PortalSettings = _appSettings.PortalSettings,
                Brands = _brandService.GetAllBrands()
            };
        }
    }
}
