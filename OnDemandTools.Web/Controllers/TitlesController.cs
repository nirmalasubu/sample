
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.User;
using OnDemandTools.Web.Models.User;
using System.Security.Claims;
using OnDemandTools.Common.Model;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using OnDemandTools.Business.Adapters.Titles;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class TitlesController : Controller
    {
        public ITitleFinder _titleFinder;
        public TitlesController(ITitleFinder titleFinder)
        {
            _titleFinder = titleFinder;
        }

        // GET: api/values
        [Authorize]
        [HttpGet("{searchterm}")]
        public string SearchTitles(string searchterm)
        {
            _titleFinder.Find(searchterm);
            return string.Empty;
        }
    }
}
