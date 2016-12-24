using AutoMapper;
using OnDemandTools.Business.Modules.AiringId.Model;
using OnDemandTools.API.v1.Models;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public class AiringIdViewModelProfile: Profile
    {
        public AiringIdViewModelProfile()
        {
            CreateMap<CurrentAiringId, CurrentAiringIdViewModel>()             
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));

            CreateMap<CurrentAiringIdViewModel, CurrentAiringId>()                          
               .ForMember(d => d.BillingNumber, opt => opt.MapFrom(s => new BillingNumber
               {
                   Current = s.BillingNumberCurrent,
                   Lower = s.BillingNumberLower,
                   Upper = s.BillingNumberUpper
               }));
        }
    }
}
