using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Common.Model;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using OnDemandTools.Business.Modules.Destination.Model;
using OnDemandTools.Web.Models.Destination;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Business.Modules.Airing.Model;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        IDestinationService _destinationSvc;
        AppSettings appsettings;

        public CategoryController(IDestinationService _destinationSvc,
            AppSettings appsettings
            )
        {
            this.appsettings = appsettings;
            this._destinationSvc = _destinationSvc;
        }

        // GET: api/values
        //[Authorize]
        [HttpGet]
        public IEnumerable<CategoryViewModel> Get()
        {
            List<CategoryViewModel> categoriesVM = new List<CategoryViewModel>();
            List<Business.Modules.Destination.Model.Destination> destinations = _destinationSvc.GetAll();
            List<DestinationViewModel> destinationsModel = destinations.ToViewModel<List<Business.Modules.Destination.Model.Destination>, List<DestinationViewModel>>();            

            var categories = destinationsModel.SelectMany(d => d.Categories).Distinct();

            foreach (var category in categories)
            {
                CategoryViewModel categoryVM = new CategoryViewModel
                {
                    Name = category.Name,
                    Destinations = destinationsModel.Where(d => d.Categories.Any(c => c.Name == category.Name)).ToList()                    
                };

                categoriesVM.Add(categoryVM);
            }

            return categoriesVM;
        }

        [Authorize]
        [HttpGet("newCategory")]
        public CategoryViewModel GetEmptyModel()
        {
            return new CategoryViewModel
            {
                Name = string.Empty           
            };
        }
    }
}
