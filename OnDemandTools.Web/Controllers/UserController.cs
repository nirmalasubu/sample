
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.User;
using OnDemandTools.Web.Models.User;
using System.Security.Claims;
using OnDemandTools.Common.Model;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        [HttpGet]
        public string Get()
        {
            ClaimsPrincipal user = _userSvc.GetByUserName(HttpContext.User.Identity.Name);
            var userDetails = user.Identities.FirstOrDefault();

            var name = HttpContext.User.Claims.FirstOrDefault(e => e.Type == "name");

            if (name != null)
            {
                var fullName = name.Value;

                var allNames = Regex.Split(fullName, ",");

                if (allNames.Length == 2)
                {
                    return string.Format("{0} {1}", allNames[1].Trim(), allNames[0].Trim());
                }
            }

            return HttpContext.User.Identity.Name;
        }
    }
}
