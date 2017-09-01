using Nancy;
using Nancy.Security;
using OnDemandTools.API.Helpers;
using ADModel = OnDemandTools.API.v1.Models.Destination;
using OnDemandTools.Business.Modules.Destination;
using OnDemandTools.Business.Modules.Destination.Model;
using OnDemandTools.Common.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using OnDemandTools.Common.Exceptions;

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
                Destination destination = desService.GetByName(name);
                return AddDestinationCategoriesToProperties(destination).ToViewModel<Destination, ADModel.Destination>();
            });

           Get("/destinations",  _ =>
            {
                this.RequiresClaims(c => c.Type == HttpMethod.Get.Verb());
                var destinations = desService.GetAll();

                // retrieve permitted destinations
                var permittedDestinations = FilterDestinations(destinations, Context.User().Destinations);

                return AddListDestinationsCategoriesToProperties(permittedDestinations).ToViewModel<List<Destination>, List<ADModel.Destination>>();
            });

        }

        #region "PRIVATE METHODS"

        /// <summary>
        /// To add category strings within the properties array in the  list of destinations
        /// </summary>
        /// <param name="permittedDestinations">destination list</param>
        /// <returns>destinations</returns>
        private List<Destination> AddListDestinationsCategoriesToProperties(List<Destination> permittedDestinations)
        {
            foreach (Destination des in permittedDestinations)  //verify each destination has categories . if yes then combine categories and properties.
            {
                if (des.Categories.Any())
                {
                    foreach (Category cat in des.Categories)
                    {                        
                        des.Properties.Add(cat.ToBusinessModel<Category, Property>());
                    }
                }
            }
            return permittedDestinations;
        }

        /// <summary>
        /// To add category strings within the properties array of destination.
        /// </summary>
        /// <param name="destination">destination</param>
        /// <returns>destination</returns>
        private Destination AddDestinationCategoriesToProperties(Destination destination)
        {
            if (destination.Categories.Any())
            {
                foreach (Category cat in destination.Categories)
                {
                     destination.Properties.Add(cat.ToBusinessModel<Category, Property>());
                }
            }
            return destination;
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
        #endregion
    }
}
