using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Common.Configuration
{    public static class ConfigurationExtensions
    {
        public static String Get(this IConfiguration configuration, String key)
        {
            return configuration.AsEnumerable()
                .Single(c => c.Key == key)
                .Value;            
        }

        public static Service GetExternalService (this AppSettings configuration, String name)
        {
            return configuration.Services
                    .Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    .Single();
        }
    }
}
