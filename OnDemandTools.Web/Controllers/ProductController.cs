using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using OnDemandTools.Common.Model;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using OnDemandTools.Business.Modules.Product.Model;
using OnDemandTools.Web.Models.Product;
using OnDemandTools.Business.Modules.Product;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnDemandTools.Web.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        IProductService productSvc;
        AppSettings appsettings;

        public ProductController(IProductService productSvc,
            AppSettings appsettings
            )
        {
            this.appsettings = appsettings;
            this.productSvc = productSvc;
        }

        // GET: api/values
        //[Authorize]
        [HttpGet]
        public IEnumerable<ProductViewModel> Get()
        {
            List<Product> products = productSvc.GetAll();
            List<ProductViewModel> productModel = products.ToViewModel<List<Product>, List<ProductViewModel>>();

            return productModel;
        }
    }
}
