using AutoMapper;
using OnDemandTools.Utilities.EntityMapping.Rules;
using System;
using System.Collections.Generic;

namespace OOnDemandTools.Utilities.EntityMapping
{

    public static class MappingBootstrapper
    {
        public static void Map(List<Profile> extraProfiles = null)
        {
            Mapper.Initialize(cfg =>
            {
               
                cfg.AddProfile<UserProfile>();
              
                try
                {
                    if (extraProfiles != null)
                    {
                        foreach (var p in extraProfiles)
                            cfg.AddProfile(p);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            });

        }
    }
}
