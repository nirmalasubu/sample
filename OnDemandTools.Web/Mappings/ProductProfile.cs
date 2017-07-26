using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using OnDemandTools.Web.Models.Product;
using BLModel = OnDemandTools.Business.Modules.Product.Model;


namespace OnDemandTools.Web.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel, BLModel.Product>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => t.Name)));
            CreateMap<ContentTier, BLModel.ContentTier>();

            CreateMap<BLModel.Product, ProductViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => new Tag(t))));
            CreateMap<BLModel.ContentTier, ContentTier>();
        }
    }
}
