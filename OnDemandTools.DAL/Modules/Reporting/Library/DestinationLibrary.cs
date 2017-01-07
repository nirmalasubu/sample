using Microsoft.Extensions.Configuration;
using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.DAL.Modules.Reporting.Model;
using OnDemandTools.DAL.Modules.Reporting.Queries;
using System;
using System.Collections.Generic;
using System.Linq;


namespace OnDemandTools.DAL.Modules.Reporting.Library
{
    public class DestinationLibrary
    {
        private List<DF_Destination> _destinations;
        AppSettings configuration;

        public DestinationLibrary(AppSettings configuration)
        {
            this.configuration = configuration;
            LoadDestinations();           
        }

        private void LoadDestinations()
        {
            var query = new DF_DestinationQuery(configuration).Get();
            _destinations = query.ToList();
        }


        public DF_Destination GetByName(string name, bool caseSensitive = false)
        {
            var destination = caseSensitive
                ? _destinations.FirstOrDefault(se => se.Name == name)
                : _destinations.FirstOrDefault(se => se.Name.ToLower() == name.ToLower());

            if (destination == null)
                throw new Exception(string.Format("Destination {0} does not exist.", name));

            return destination;
        }

        public DF_Destination GetByEnum(int? destinationEnum)
        {
            var destination = _destinations.FirstOrDefault(se => se.DestinationID == destinationEnum);

            if (destination == null)
                throw new Exception(string.Format("DestinationId {0} does not exist.", destinationEnum));

            return destination;
        }

        public DF_Destination TryGetByEnum(int? destinationEnum)
        {
            return _destinations.FirstOrDefault(se => se.DestinationID == destinationEnum);
        }

        public string TryGetNameByEnum(int? destinationEnum)
        {
            var destination = _destinations.FirstOrDefault(se => se.DestinationID == destinationEnum);

            return destination == null
                ? "Destination Missing"
                : destination.Name;
        }

        public List<DF_Destination> GetDestinations()
        {
            return _destinations;
        }

    }
}
