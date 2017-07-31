using AutoMapper;
using OnDemandTools.Web.Models.Distribution;
using OnDemandTools.Web.Models.Status;
using BLModel = OnDemandTools.Business.Modules.AiringId.Model;


namespace OnDemandTools.Web.Mappings
{
    public class AiringIdProfile : Profile
    {
        public AiringIdProfile()
        {
            CreateMap<BLModel.CurrentAiringId, CurrentAiringIdViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()))
                .ForMember(d => d.BillingNumberCurrent, opt => opt.MapFrom(s => s.BillingNumber.Current))
                .ForMember(d => d.BillingNumberLower, opt => opt.MapFrom(s => s.BillingNumber.Lower))
                .ForMember(d => d.BillingNumberUpper, opt => opt.MapFrom(s => s.BillingNumber.Upper));

            CreateMap<CurrentAiringIdViewModel, BLModel.CurrentAiringId>()
                .ForMember(d => d.BillingNumber, opt => opt.MapFrom(s => new BLModel.BillingNumber
                {
                    Current = s.BillingNumberCurrent,
                    Lower = s.BillingNumberLower,
                    Upper = s.BillingNumberUpper
                }));
        }
    }
}
