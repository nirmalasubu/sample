using AutoMapper;

namespace OnDemandTools.API.Helpers.MappingRules
{
    public static class AutoMapperAPIConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new AiringIdViewModelProfile());
                cfg.AddProfile(new DestinationViewModelProfile());
                cfg.AddProfile(new ProductViewModelProfile());

                cfg.CreateMissingTypeMaps = true;
            });
        }
    }
}
