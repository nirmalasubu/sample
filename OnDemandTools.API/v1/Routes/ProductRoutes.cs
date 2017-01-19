using Nancy;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using ADModel = OnDemandTools.API.v1.Models.Destination;
using RQModel = OnDemandTools.API.v1.Models.Product;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Business.Modules.Destination.Model;
using OnDemandTools.Business.Modules.Product;
using OnDemandTools.Business.Modules.Product.Model;
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OnDemandTools.API.v1.Routes
{

    public class ProductRoutes : NancyModule
    {
        public ProductRoutes(IProductService productSvc, IDestinationService destinationSvc)
            : base("v1")
        {
            this.RequiresAuthentication();

            Get("/products", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var tagParameter = (DynamicDictionaryValue)Request.Query["tags"];
                var tags = ConvertParameterToTags(tagParameter);

                var products = (tagParameter.HasValue)
                    ? productSvc.GetByTags(tags).ToViewModel<List<Product>,List<RQModel.Product>>()
                    : productSvc.GetAll().ToViewModel<List<Product>, List<RQModel.Product>>();

                return products;               
            });

            Get("/product/{productId}/destinations",  _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var destinations = destinationSvc
                                  .GetByProductId((Guid)_.productId)
                                  .ToViewModel<List<Destination>, List<ADModel.Destination>>();

                return destinations;
            });


            Get("/product/mapping/{mappingId}/destinations",  _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var destinations = destinationSvc
                                    .GetByMappingId((int)_.mappingId)
                                    .ToViewModel<List<Destination>, List<ADModel.Destination>>();

                return destinations;
            });

        }

        private List<string> ConvertParameterToTags(dynamic tagParameter)
        {           
            return (tagParameter.HasValue)
                ? ((string)tagParameter).Split('|')
                .ToList<String>().Select(c => c.Replace(" ", "-")).ToList()
                : new List<string>();
        }
    }
}
