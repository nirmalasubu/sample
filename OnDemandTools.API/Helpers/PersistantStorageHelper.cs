using Microsoft.Extensions.DependencyInjection;
using OnDemandTools.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.Helpers
{
    public static class PersistantStorageHelper
    {
        public static void InitializePersistantStorage(this IServiceCollection services)
        {
            // Initialize everything needed for MongoDB persistant storage
            MongoBoostrapper.Setup();
        }
    }
}
