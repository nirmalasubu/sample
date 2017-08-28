using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Web.Models.UserPermissions;
using OnDemandTools.Business.Modules.UserPermissions;
using OnDemandTools.Common.Model;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using Microsoft.AspNetCore.Authorization;

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


        [Authorize]
        [HttpGet]        
        public IEnumerable<UserPermission> Get()
        {
            return _service.GetAll(UserType.Portal).OrderBy(e => e.UserName).ToList()
            .ToViewModel<List<BLModel.UserPermission>, List<UserPermission>>();
        }


        [Authorize]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Authorize]
        [HttpPost]
        public UserPermission Post([FromBody]UserPermission viewModel)
        {


            if (string.IsNullOrEmpty(viewModel.Id))
            {
                viewModel.Api.ApiKey = Guid.NewGuid().ToString();
                viewModel.CreatedDateTime = DateTime.UtcNow;
                viewModel.CreatedBy = HttpContext.User.Identity.Name;
            }
            else
            {
                viewModel.ModifiedDateTime = DateTime.UtcNow;
                viewModel.ModifiedBy = HttpContext.User.Identity.Name;
            }
            BLModel.UserPermission model = _service.Save(viewModel.ToBusinessModel<UserPermission, BLModel.UserPermission>());
           

            return model.ToViewModel<BLModel.UserPermission, UserPermission>();
        }

        [Authorize]
        [HttpGet("newuserpermission")]
        public UserPermission GetEmptyModel()
        {
            return new UserPermission
            {
                UserName =string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                PhoneNumber = string.Empty,
                Extension = string.Empty,
                Notes = string.Empty,
                Portal=new Portal { IsActive=true},
                Api = new Api {ApiKey="" }
            };
        }
    }
}
