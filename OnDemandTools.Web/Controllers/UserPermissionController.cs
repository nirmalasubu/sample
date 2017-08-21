using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Web.Models.UserPermissions;
using OnDemandTools.Business.Modules.UserPermissions;
using OnDemandTools.Common.Model;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserPermissionController : Controller
    {

        IUserPermissionService _service;

        public UserPermissionController(IUserPermissionService service)
        {
            _service = service;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<UserPermission> Get()
        {
            return _service.GetAll(UserType.Portal).ToViewModel<List<BLModel.UserPermission>, List<UserPermission>>();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }       
    }
}
