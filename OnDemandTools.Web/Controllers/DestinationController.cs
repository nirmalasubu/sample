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
    public class DestinationController : Controller
    {
        IDestinationService _destinationSvc;
        AppSettings appsettings;

        public DestinationController(IDestinationService destinationSvc,
            AppSettings appsettings
            )
        {
            this.appsettings = appsettings;
            _destinationSvc = destinationSvc;
        }

        // GET: api/values
        //[Authorize]
        [HttpGet]
        public IEnumerable<DestinationViewModel> Get()
        {
            List<Business.Modules.Destination.Model.Destination> destinations = _destinationSvc.GetAll();
            List<DestinationViewModel> destinationModel = destinations.ToViewModel<List<Business.Modules.Destination.Model.Destination>, List<DestinationViewModel>>();
           foreach(DestinationViewModel destination in destinationModel)
            {
                if(destination.Properties.Any())
                {
                    foreach(Web.Models.Destination.Property property in destination.Properties)
                    {
                        if(property.TitleIds.Any())
                        {
                            List<Business.Modules.Airing.Model.Alternate.Title.Title> titles =  _destinationSvc.GetTitlesNameFor(property.TitleIds);
                            property.Titles = titles.ToViewModel <List<Business.Modules.Airing.Model.Alternate.Title.Title>, List<Models.Destination.Title>>();
                        }
                        if (property.SeriesIds.Any())
                        {
                            List<Business.Modules.Airing.Model.Alternate.Title.Title> titles = _destinationSvc.GetTitlesNameFor(property.SeriesIds);
                            if(property.Titles.Any())
                                property.Titles.AddRange(titles.ToViewModel<List<Business.Modules.Airing.Model.Alternate.Title.Title>, List<Models.Destination.Title>>());
                            else
                                property.Titles = titles.ToViewModel<List<Business.Modules.Airing.Model.Alternate.Title.Title>, List<Models.Destination.Title>>();
                        }
                    }
                }

                if (destination.Categories.Any())
                {
                    foreach (Web.Models.Destination.Category category in destination.Categories)
                    {
                        if (category.TitleIds.Any())
                        {
                            List<Business.Modules.Airing.Model.Alternate.Title.Title> titles = _destinationSvc.GetTitlesNameFor(category.TitleIds);
                            category.Titles = titles.ToViewModel<List<Business.Modules.Airing.Model.Alternate.Title.Title>, List<Models.Destination.Title>>();
                        }
                        if (category.SeriesIds.Any())
                        {
                            List<Business.Modules.Airing.Model.Alternate.Title.Title> titles = _destinationSvc.GetTitlesNameFor(category.SeriesIds);
                            if (category.Titles.Any())
                                category.Titles.AddRange(titles.ToViewModel<List<Business.Modules.Airing.Model.Alternate.Title.Title>, List<Models.Destination.Title>>());
                            else
                                category.Titles = titles.ToViewModel<List<Business.Modules.Airing.Model.Alternate.Title.Title>, List<Models.Destination.Title>>();
                        }
                    }
                }
            }
            return destinationModel;
        }

        [Authorize]
        [HttpGet("newdestination")]
        public DestinationViewModel GetEmptyModel()
        {
            return new DestinationViewModel
            {
                Name = string.Empty,
                Description = string.Empty,
                ExternalId = 0                
            };
        }

        //save destination details
        [Authorize]
        [HttpPost]
        public DestinationViewModel Post([FromBody]DestinationViewModel viewModel)
        {
            Business.Modules.Destination.Model.Destination blModel = viewModel.ToBusinessModel<DestinationViewModel, Business.Modules.Destination.Model.Destination>();

            if (string.IsNullOrEmpty(blModel.Id))
            {
                blModel.CreatedDateTime = DateTime.UtcNow;
                blModel.CreatedBy = HttpContext.User.Identity.Name;
            }
            else
            {
                blModel.ModifiedDateTime = DateTime.UtcNow;
                blModel.ModifiedBy = HttpContext.User.Identity.Name;
            }

            blModel = _destinationSvc.Save(blModel);

            return blModel.ToViewModel<Business.Modules.Destination.Model.Destination, DestinationViewModel>();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _destinationSvc.Delete(id);
        }
    }
}
