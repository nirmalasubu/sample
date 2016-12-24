using AutoMapper;
using OnDemandTools.API.v1.Models.Product;
using OnDemandTools.Business.Modules.Product.Model;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class ProductViewModelProfile: Profile
    {
        public ProductViewModelProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => new TagViewModel(t))));

            CreateMap<ProductViewModel, Product>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => t.Text)));
        }
    }


    public class TagsResolver : IValueResolver<Product, ProductViewModel, List<TagViewModel>>
    {
        public List<TagViewModel> Resolve(Product src, ProductViewModel des, List<TagViewModel> d, ResolutionContext context)
        {
            foreach (string c in src.Tags)
            {
                des.Tags.Add(new TagViewModel() { Text = c });                
            }

            return des.Tags;
        }
    }
}
