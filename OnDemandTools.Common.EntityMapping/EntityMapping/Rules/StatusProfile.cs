using AutoMapper;
using MongoDB.Bson;
using OnDemandTools.DAL.Modules.Status.Model;

namespace OnDemandTools.Common.EntityMapping.EntityMapping.Rules
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            // Mapping status (data model) to status (business/view model)
            CreateMap<Status, Business.Modules.Status.Model.Status>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            // Mapping status (business/view model) to status (data model)
            CreateMap<Business.Modules.Status.Model.Status, Status>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id != null ? new ObjectId(src.Id) : ObjectId.Empty));
        }
    }
}