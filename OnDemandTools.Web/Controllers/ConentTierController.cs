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
                    .ToViewModel<List<Business.Modules.Product.Model.Product>, List<Models.Product.ProductViewModel>>()
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
    }
}
