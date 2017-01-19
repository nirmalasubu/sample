using AutoMapper;
using RQModel = OnDemandTools.API.v1.Models.Product;
using OnDemandTools.Business.Modules.Product.Model;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class ProductViewModelProfile: Profile
    {
        public ProductViewModelProfile()
        {
            CreateMap<Product, RQModel.Product>();

            CreateMap<RQModel.Product, Product>();
        }
    }
}
