using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace OnDemandTools.Common.Configuration
{
    public static class ConfigurationExtensions
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

        public static T Get<T>(this IConfiguration config, string key) where T : new()
        {
            var instance = new T();
            config.GetSection(key).Bind(instance);
            return instance;
        }
    }
}
