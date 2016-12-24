using AutoMapper;
using OnDemandTools.Utilities.EntityMapping.Rules;
using System;
using System.Collections.Generic;

namespace OnDemandTools.Utilities.EntityMapping
{

    public static class AutoMapperDomainConfiguration
    {

        public static void Configure()
        {
           
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new AiringIdProfile());
                cfg.AddProfile(new DestinationProfile());
                cfg.AddProfile(new ProductProfile());


                //cfg.AddProfiles(new[]
                //{
                //    typeof(OnDemandTools.Utilities.EntityMapping.Rules.UserProfile)
                //});

                cfg.CreateMissingTypeMaps = true;
            });

           
        }

    }
}
