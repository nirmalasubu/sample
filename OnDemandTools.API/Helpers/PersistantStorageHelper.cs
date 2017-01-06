using Microsoft.Extensions.DependencyInjection;
using OnDemandTools.API.Utilities;

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
