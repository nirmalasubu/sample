using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.UserPermissions;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.UserPermissions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public IUserPermissionService _userSvc;
        public UserController(IUserPermissionService userSvc)
        {
            _userSvc = userSvc;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public UserPermission Get()
        {
            UserPermission user = _userSvc.GetByUserName(HttpContext.User.Identity.Name).ToViewModel<Business.Modules.UserPermissions.Model.UserPermission, UserPermission>();

            return user;
        }
    }
}
