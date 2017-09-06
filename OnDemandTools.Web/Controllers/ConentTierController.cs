using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Model;
using OnDemandTools.Web.Models.Destination;
using System.Collections.Generic;
using System.Linq;
using System;
using OnDemandTools.Business.Modules.Product;
using OnDemandTools.Web.Models.ContentTier;
using OnDemandTools.Web.Models.Product;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class ContentTierController : Controller
    {
        IProductService _productSvc;
        AppSettings appsettings;

        public ContentTierController(IProductService productSvc,
            AppSettings appsettings
            )
        {
            this.appsettings = appsettings;
            this._productSvc = productSvc;
        }

        // GET: api/values
        [Authorize]
        [HttpGet]
        public IEnumerable<ContentTierViewModel> Get()
        {
            List<ContentTierViewModel> contentTiersVM = new List<ContentTierViewModel>();
            var allProducts = _productSvc.GetAll();

            var contentTiers = allProducts.SelectMany(d => d.ContentTiers);

            foreach (var contentTier in contentTiers.GroupBy(e => e.Name))
            {
                ContentTierViewModel contentTierVm = new ContentTierViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = contentTier.Key,
                    Products = allProducts.Where(e => e.ContentTiers.Any(f => f.Name == contentTier.Key)).ToList()
                    .ToViewModel<List<Business.Modules.Product.Model.Product>, List<ProductViewModel>>()
                };

                foreach (var product in contentTierVm.Products)
                {
                    product.ContentTiers = new List<Models.Product.ContentTier> { product.ContentTiers.First(e => e.Name == contentTier.Key) };
                }

                contentTiersVM.Add(contentTierVm);
            }

            return contentTiersVM;
        }

        [Authorize]
        [HttpGet("newContentTier")]
        public ContentTierViewModel GetEmptyModel()
        {
            return new ContentTierViewModel
            {
                Name = string.Empty
            };
        }

        //save destination details
        [Authorize]
        [HttpPost]
        public ContentTierViewModel Post([FromBody]ContentTierViewModel viewModel)
        {
            List<ProductViewModel> destinations = new List<ProductViewModel>();

            foreach (var product in viewModel.Products)
            {
                Business.Modules.Product.Model.Product productDetail = null;

                if (!string.IsNullOrEmpty(product.Name))
                    productDetail = _productSvc.GetById(product.ExternalId);
                if (productDetail != null)
                {
                    if (!string.IsNullOrEmpty(product.ContentTiers.First().Name))
                    {
                        if (!string.IsNullOrEmpty(product.ContentTiers.First().Id))
                        {
                            var category = productDetail.ContentTiers.FirstOrDefault(e => e.Id == product.ContentTiers.First().Id);

                            category.Name = product.ContentTiers.First().Name;
                            category.Brands = product.ContentTiers.First().Brands;
                            category.TitleIds = product.ContentTiers.First().TitleIds;
                        }
                        else
                        {
                            var contentTier = new Business.Modules.Product.Model.ContentTier
                            {
                                Name = product.ContentTiers.First().Name,
                                Brands = product.ContentTiers.First().Brands,
                                TitleIds = product.ContentTiers.First().TitleIds
                            };

                            productDetail.ContentTiers.Add(contentTier);
                        }
                    }
                    else
                        productDetail.ContentTiers.RemoveAll(e => e.Id == product.ContentTiers.First().Id);

                    var blModel = _productSvc.Save(productDetail);

                    destinations.Add(blModel.ToViewModel<Business.Modules.Product.Model.Product, ProductViewModel>());
                }
            }

            var newModel = new ContentTierViewModel();
            newModel.Id = viewModel.Id;
            if (newModel.Id == null)
            {
                newModel.Id = new Guid().ToString();
            }
            newModel.Name = viewModel.Name;

            var newProducts = new List<ProductViewModel>();

            foreach (var product in viewModel.Products)
            {
                if (product.ContentTiers.Any() && !string.IsNullOrWhiteSpace(product.ContentTiers.First().Name))
                    newProducts.Add(product);
            }

            newModel.Products = newProducts;

            return newModel;
        }

        [HttpPost("import")]
        public string ImportCategory([FromBody]ContentTierImportViewModel viewModel)
        {

            var product = _productSvc.GetByMappingId(viewModel.MappingId);

            if (product != null)
            {
                bool hasChanges = false;
                foreach (var contentTier in viewModel.ContentTiers)
                {
                    if (!product.ContentTiers.Any(e => e.Name == contentTier.Name))
                    {
                        hasChanges = true;
                        var newCategory = new Business.Modules.Product.Model.ContentTier
                        {
                            Name = contentTier.Name,
                            Brands = contentTier.Brands,
                            TitleIds = contentTier.TitleIds
                        };

                        product.ContentTiers.Add(newCategory);
                    }

                    if (hasChanges)
                        _productSvc.Save(product);
                }
            }


            return "Success";
        }

        [Authorize]
        [HttpDelete]
        public bool Delete([FromBody]ContentTierViewModel viewModel)
        {
            _productSvc.DeleteContentTierByName(viewModel.Name);
            return true;
        }
    }
}
