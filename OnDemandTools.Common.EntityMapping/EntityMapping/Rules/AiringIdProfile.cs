using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.AiringId.Model;
using DLModel = OnDemandTools.DAL.Modules.AiringId.Model;

namespace OnDemandTools.Common.EntityMapping
{
    public class AiringIdProfile: Profile
    {
        public AiringIdProfile()
        {
            CreateMap<BLModel.CurrentAiringId, DLModel.CurrentAiringId>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => ObjectId.Parse(s.Id)))
              .ForMember(d => d.BillingNumber, opt => opt.MapFrom(s => s.BillingNumber));

            CreateMap<DLModel.CurrentAiringId, BLModel.CurrentAiringId>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()))
             .ForMember(d => d.BillingNumber, opt => opt.MapFrom(s => s.BillingNumber));

            CreateMap<BLModel.BillingNumber, DLModel.BillingNumber>();
            CreateMap<DLModel.BillingNumber, BLModel.BillingNumber>();
        }
    }
}
