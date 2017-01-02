using AutoMapper;
using BLTitleModel = OnDemandTools.Business.Modules.Airing.Model.Alternate.Title;
using VMTitleModel = OnDemandTools.API.v1.Models.Airing.Title;

namespace OnDemandTools.API.Helpers.MappingRules.TitleProfile
{
    public class TitleProfile: Profile
    {
        public TitleProfile()
        {
            // long BL to VM
            CreateMap<BLTitleModel.Title, VMTitleModel.Title>();
            CreateMap<BLTitleModel.Participant, VMTitleModel.Participant>();
            CreateMap<BLTitleModel.Rating, VMTitleModel.Rating>();
            CreateMap<BLTitleModel.RatingDescriptor, VMTitleModel.RatingDescriptor>();
            CreateMap<BLTitleModel.OtherName, VMTitleModel.OtherName>();
            CreateMap<BLTitleModel.Storyline, VMTitleModel.Storyline>();
            CreateMap<BLTitleModel.ExternalSource, VMTitleModel.ExternalSource>();                 
        }
    }
}
