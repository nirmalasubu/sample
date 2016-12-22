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

                cfg.CreateMissingTypeMaps = true;
            });
        }

    }
}
