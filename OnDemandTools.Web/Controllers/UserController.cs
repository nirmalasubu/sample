
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.User;
using OnDemandTools.Web.Models.User;
using System.Security.Claims;
using OnDemandTools.Common.Model;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public IUserHelper _userSvc;
        public UserController(IUserHelper userSvc)
        {
            _userSvc = userSvc;
        }
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            ClaimsPrincipal user = _userSvc.GetByUserName(HttpContext.User.Identity.Name);
            var userDetails = user.Identities.FirstOrDefault();

            return "Test";
        }
    }
}
