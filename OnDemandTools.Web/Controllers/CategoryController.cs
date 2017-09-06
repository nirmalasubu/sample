using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.Destination;
using System.Collections.Generic;
using System.Linq;
using System;
using OnDemandTools.Web.Models.Category;


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
        [Authorize]
        [HttpGet]
        public IEnumerable<CategoryViewModel> Get()
        {
            List<CategoryViewModel> categoriesVM = new List<CategoryViewModel>();
            List<Business.Modules.Destination.Model.Destination> destinations = _destinationSvc.GetAll();

            var categories = destinations.SelectMany(d => d.Categories);

            foreach (var category in categories.GroupBy(e => e.Name))
            {
                CategoryViewModel categoryVM = new CategoryViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = category.Key,
                    Destinations = destinations.Where(e => e.Categories.Any(f => f.Name == category.Key)).ToList()
                    .ToViewModel<List<Business.Modules.Destination.Model.Destination>, List<DestinationViewModel>>()
                };

                foreach (var destination in categoryVM.Destinations)
                {
                    destination.Categories = new List<Category> { destination.Categories.First(e => e.Name == category.Key) };
                }

                categoriesVM.Add(categoryVM);
            }

            return categoriesVM;
        }

        //save destination details
        [Authorize]
        [HttpPost]
        public CategoryViewModel Post([FromBody]CategoryViewModel viewModel)
        {
            List<DestinationViewModel> destinations = new List<DestinationViewModel>();

            foreach (var destination in viewModel.Destinations)
            {
                Business.Modules.Destination.Model.Destination destinationDetail = null;

                if (!string.IsNullOrEmpty(destination.Name))
                    destinationDetail = _destinationSvc.GetByName(destination.Name);
                if (destinationDetail != null)
                {
                    if (!string.IsNullOrEmpty(destination.Categories.First().Name))
                    {
                        if (!string.IsNullOrEmpty(destination.Categories.First().Id))
                        {
                            var category = destinationDetail.Categories.FirstOrDefault(e => e.Id == destination.Categories.First().Id);

                            category.Name = destination.Categories.First().Name;
                            category.Brands = destination.Categories.First().Brands;
                            category.TitleIds = destination.Categories.First().TitleIds;
                        }
                        else
                        {
                            var newCategory = new Business.Modules.Destination.Model.Category
                            {
                                Name = destination.Categories.First().Name,
                                Brands = destination.Categories.First().Brands,
                                TitleIds = destination.Categories.First().TitleIds
                            };

                            destinationDetail.Categories.Add(newCategory);
                        }
                    }
                    else
                        destinationDetail.Categories.RemoveAll(e => e.Id == destination.Categories.First().Id);

                    var blModel = _destinationSvc.Save(destinationDetail);

                    destinations.Add(blModel.ToViewModel<Business.Modules.Destination.Model.Destination, DestinationViewModel>());
                }
            }

            if (string.IsNullOrEmpty(viewModel.Id))
                viewModel.Id = Guid.NewGuid().ToString();

            viewModel.Destinations = destinations.Where(d => d.Categories.Count() > 0).ToList();

            foreach (var destination in viewModel.Destinations)
            {
                destination.Categories = new List<Category> { destination.Categories.FirstOrDefault(e => e.Name == viewModel.Name) };
            }

            viewModel.Destinations = viewModel.Destinations.Where(d => d.Categories.FirstOrDefault() != null).ToList();

            return viewModel;
        }

        [HttpPost("import")]
        public string ImportCategory([FromBody]CategoryImportViewModel viewModel)
        {
            foreach (var category in viewModel.Categories)
            {
                category.Id = Guid.NewGuid().ToString();
            }

            var destinations = _destinationSvc.GetByMappingId(viewModel.MappingId);

            foreach (var destination in destinations)
            {
                bool hasChanges = false;
                foreach (var category in viewModel.Categories)
                {
                    if (!destination.Categories.Any(e => e.Name == category.Name))
                    {
                        hasChanges = true;
                        var newCategory = new Business.Modules.Destination.Model.Category
                        {
                            Name = category.Name,
                            Brands = category.Brands,
                            TitleIds = category.TitleIds
                        };

                        destination.Categories.Add(newCategory);
                    }
                }

                if (hasChanges)
                    _destinationSvc.Save(destination);
            }


            return "Success";
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

        [Authorize]
        [HttpDelete]
        public bool Delete([FromBody]CategoryViewModel viewModel)
        {
            _destinationSvc.DeleteCategoryByName(viewModel.Name);
            return true;
        }
    }
}
