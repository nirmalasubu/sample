using Nancy;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using ADModel = OnDemandTools.API.v1.Models.Destination;
using OnDemandTools.Business.Modules.CustomExceptions;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Business.Modules.Destination.Model;
using OnDemandTools.Common.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OnDemandTools.API.v1.Routes
{

    public class DestinationRoutes : NancyModule
    {
        public DestinationRoutes(IDestinationService desService)
            : base("v1")
        {
            this.RequiresAuthentication();

            Get("/destination/{name}", _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());                
                var name = (string)_.name;
                ValidateRequest(name, Context.User().Destinations);

                return desService.GetByName(name).ToViewModel<Destination, ADModel.Destination>();
            });

            Get("/destinations",  _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var destinations = desService.GetAll();

                // retrieve permitted destinations
                var permittedDestinations = FilterDestinations(destinations, Context.User().Destinations);

                return permittedDestinations.ToViewModel<List<Destination>, List<ADModel.Destination>>();
            });

        }

        private List<Destination> FilterDestinations(IEnumerable<Destination> destinations, IEnumerable<string> permittedDestinations)
        {
            return destinations.Where(d => permittedDestinations.Contains(d.Name)).ToList();
        }

        private void ValidateRequest(string name, IEnumerable<string> permittedDestinations)
        {
            if (!permittedDestinations.Contains(name))
                throw new SecurityAccessDeniedException(string.Format("Request denied for {0} destination.", name));
        }

    }
}
