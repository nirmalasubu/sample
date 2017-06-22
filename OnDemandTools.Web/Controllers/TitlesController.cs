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
using OnDemandTools.Web.Models.TitleSearch;
using AutoMapper;
using OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using System.Collections.Generic;

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class TitlesController : Controller
    {
        ITitleFinder _titleFinder;
        IFlowTitles _flowTitles;
        public TitlesController(ITitleFinder titleFinder, IFlowTitles flowTitles)
        {
            _titleFinder = titleFinder;
            _flowTitles = flowTitles;
        }

        // GET: api/values
        [Authorize]
        [HttpGet("search/{searchterm}")]
        public TitleSearchResults SearchTitles(string searchterm)
        {
            TitleSearchResults results = new TitleSearchResults();

            results.Titles = _titleFinder.Find(searchterm).Select(e => e.ToViewModel<Title, TitleShort>()).ToList();

            results.TitleTypeFilterParameters = results.Titles.GroupBy(e => e.TitleType.Name)
                 .Select(y => new TitleFilterParameter { Name = y.Key.ToString(), Count = y.Count() })
                 .OrderByDescending(e => e.Count)
                 .ToList();

            results.SeriesFilterParameters = results.Titles.Where(e => e.SeriesTitleName != null).GroupBy(e => e.SeriesTitleName)
                .Select(y => new TitleFilterParameter { Name = y.Key.ToString(), Count = y.Count() })
                .OrderByDescending(e => e.Count)
                .ToList();

            return results;
        }

        [Authorize]
        [HttpGet("searchByTitleIds")]
        public List<TitleShort> SearchByTitleIds([FromQuery]int[] ids)
        {
            var titles = _flowTitles.GetFlowTitlesFor(ids);
            return titles.Select(e => e.ToViewModel<Title, TitleShort>()).ToList();
        }
    }
}
