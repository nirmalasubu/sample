using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Business.Modules.Brands;
using OnDemandTools.Web.Models.Config;
using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.UserPermissions;
using OnDemandTools.Business.Modules.UserPermissions;
using System;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        AppSettings _appSettings;
        IBrandService _brandService;
        IUserPermissionService _userPermissions;

        public ConfigController(AppSettings appSettings, IBrandService brandService, IUserPermissionService userPermissions)
        {
            _appSettings = appSettings;
            _brandService = brandService;
            _userPermissions = userPermissions;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public ConfigModel Get()
        {
            return new ConfigModel
            {               
                PortalSettings = _appSettings.PortalSettings,
                PortalModules = _userPermissions.GetAllPortalModules().ToViewModel<List<BLModel.PortalModule>, List<PortalModule>>(),
                Brands = _brandService.GetAllBrands(),
                SessionExpirationTimeMinutes = int.Parse(_appSettings.SessionExpirationTimeMinutes)

            };
        }

        [Authorize]
        [HttpGet("check")]
        public string HealthCheck()
        {
            return "ok";
        }
    }
}
