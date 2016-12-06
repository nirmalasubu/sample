using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ODTPOCHarbor.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODTPOCHarbor.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {


        public ProductController(IProductRepository products)
        {
            Products = products;
        }
        public IProductRepository Products { get; set; }


        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            LogzIOServiceHelper log = new LogzIOServiceHelper();
            log.Send(new Dictionary<string, object>() { { "message", "got all products"} });

            var products = Products.GetAll();
            return products;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product pr)
        {
            LogzIOServiceHelper log = new LogzIOServiceHelper();
            log.Send(new Dictionary<string, object>() { { "message", "added product" } });

            if (pr == null)
            {
                return BadRequest();
            }
            Products.Add(pr);
            return new JsonResult(pr);
        }

    }
}
