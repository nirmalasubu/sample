using AutoMapper;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public static class AutoMapperAPIConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new AiringIdProfile());
                cfg.AddProfile(new DestinationProfile());
            });
        }
    }
}
