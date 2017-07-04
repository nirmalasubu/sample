﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.Destination;
using System.Collections.Generic;
using System.Linq;
using System;


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
                var destinationDetail = _destinationSvc.GetByName(destination.Name);

                var category = destinationDetail.Categories.FirstOrDefault(e => e.Name == destination.Categories.First().Name);

                if (category != null)
                {
                    category.Name = destination.Categories.First().Name;
                    category.Brands = destination.Categories.First().Brands;
                    category.TitleIds = destination.Categories.First().TitleIds;
                    category.SeriesIds = destination.Categories.First().SeriesIds;
                }
                else
                {
                    var newCategory = new Business.Modules.Destination.Model.Category
                    {
                        Name = destination.Categories.First().Name,
                        Brands = destination.Categories.First().Brands,
                        TitleIds = destination.Categories.First().TitleIds,
                        SeriesIds = destination.Categories.First().SeriesIds
                    };

                    destinationDetail.Categories.Add(newCategory);
                }

                var blModel = _destinationSvc.Save(destinationDetail);

                destinations.Add(blModel.ToViewModel<Business.Modules.Destination.Model.Destination, DestinationViewModel>());
            }

            viewModel.Destinations = destinations;

            foreach (var destination in viewModel.Destinations)
            {
                destination.Categories = new List<Category> { destination.Categories.First(e => e.Name == viewModel.Name) };
            }

            return viewModel;
        }

        [Authorize]
        [HttpGet("newCategory")]
        public CategoryViewModel GetEmptyModel()
        {
            return new CategoryViewModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = string.Empty
            };
        }

        [Authorize]
        [HttpDelete("{name}")]
        public bool Delete( string name)
        {
            _destinationSvc.DeleteCategoryByName(name);
            return true;
        }
    }
}