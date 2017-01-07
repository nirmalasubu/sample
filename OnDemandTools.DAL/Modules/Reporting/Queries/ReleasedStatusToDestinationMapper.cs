using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.Common.Configuration;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Reporting.Library;
using OnDemandTools.DAL.Modules.Reporting.Model;
using System.Collections.Generic;
using System.Linq;


namespace OnDemandTools.DAL.Modules.Reporting.Queries
{
    public class ReleasedStatusToDestinationMapper
    {
        readonly ODTDatastore dbConnection;

        public ReleasedStatusToDestinationMapper(AppSettings configuration)
        {
            dbConnection = new ODTDatastore(configuration);
        }

        #region public methods

        public List<DF_Status> CreateDestinationStatuses(DF_Status status, IList<DF_Destination> destinations)
        {
            var releasedDestinationStatuses = new List<DF_Status>();
            var currentFeedDestinations = GetDestinations(status.AssetID, destinations).ToList();
            List<DF_Status> dd = GetStatusesByAssetId(status.AssetID);
            var releasedStatuses = dd.Where(s => s.StatusEnum == StatusLibrary.GetStatusEnumByValue("Released").Enum).ToList();

            var existingReportedDestinations = GetReportedDestinations(releasedStatuses);
            var existingInvalidDestinations = GetExistingInvalidDestinations(dd);
            var newInvalidDestinations = GetNewInvalidDestinations(existingReportedDestinations, currentFeedDestinations, existingInvalidDestinations);
            releasedDestinationStatuses.AddRange(CreateAssetStatusesForNewDestinations(status, currentFeedDestinations));
            releasedDestinationStatuses.AddRange(CreateInvalidDestinationStatuses(status, newInvalidDestinations));
            return releasedDestinationStatuses;
        }

        #endregion

        #region private methods

        private IEnumerable<int> GetNewInvalidDestinations(IEnumerable<int> reportedDestinations, IEnumerable<int> currentDestinations, IEnumerable<int> existingInvalidDestinations)
        {
            return reportedDestinations
                .Where(d => !currentDestinations.Contains(d))
                .Where(d => !existingInvalidDestinations.Contains(d));
        }

        private IEnumerable<int> GetDestinations(string assetId, IEnumerable<DF_Destination> destinations)
        {
            Airing releasedAsset = ReleasedAssetByAssetId(assetId).FirstOrDefault();

            if (releasedAsset == null)
            {
                return new List<int>();
            }
            List<string> des = GetDestinations(releasedAsset).ToList();
            var mappedDestinations = destinations.Where(d => des.Contains(d.Name));

            return mappedDestinations.Where(d => ShouldAssetBeDeliveredToDestination(releasedAsset, d))
                .Select(d => d.DestinationID)
                .Distinct();
        }

        private bool ShouldAssetBeDeliveredToDestination(Airing releasedAsset, DF_Destination destination)
        {
            #region setup to made logic easier to read

            var isHd = releasedAsset.Flags.Hd;
            var isCx = releasedAsset.Flags.Cx;

            var acceptsCxContent = destination.AcceptsCXContent.HasValue && destination.AcceptsCXContent.Value;
            var acceptsNonCxContent = destination.AcceptsNCXContent.HasValue && destination.AcceptsNCXContent.Value;
            var acceptsHdContent = destination.AcceptsHDContent.HasValue && destination.AcceptsHDContent.Value;
            var acceptsSdContent = destination.AcceptsSDContent.HasValue && destination.AcceptsSDContent.Value;

            #endregion

            return ((acceptsCxContent && acceptsHdContent && isHd && isCx) ||
                    (acceptsCxContent && acceptsSdContent && !isHd && isCx) ||
                    (acceptsNonCxContent && acceptsHdContent && isHd && !isCx) ||
                    (acceptsNonCxContent && acceptsSdContent && !isHd && !isCx));
        }

        private IEnumerable<int> GetExistingInvalidDestinations(IEnumerable<DF_Status> releasedStatuses)
        {
            var lastDestinationStatuses = releasedStatuses
                .GroupBy(rs => rs.DestinationID)
                .Select(d =>
                    new
                    {
                        DestinationEnum = d.Key,
                        LastStatus = d.OrderByDescending(s => s.CreatedDate).FirstOrDefault()
                    });

            return lastDestinationStatuses
                .Where(d => d.LastStatus != null && d.LastStatus.StatusEnum == StatusLibrary.GetStatusEnumByValue("IgnoreDestination").Enum)
                .Select(d => d.DestinationEnum ?? 0).ToList();
        }

        private static IEnumerable<int> GetReportedDestinations(IEnumerable<DF_Status> releasedStatuses)
        {
            return releasedStatuses.Select(releasedStatus => releasedStatus.DestinationID ?? 0).Distinct().ToList();
        }

        private IEnumerable<DF_Status> CreateAssetStatusesForNewDestinations(DF_Status status, IEnumerable<int> newDestinations)
        {
            var destinationStatuses = new List<DF_Status>();

            foreach (var destinationEnum in newDestinations)
            {
                var destinationStatus = new DF_Status
                {
                    AssetID = status.AssetID,
                    DestinationID = destinationEnum,
                    CreatedBy = status.CreatedBy,
                    ModifiedBy = status.ModifiedBy,
                    CreatedDate = status.CreatedDate,
                    ModifiedDate = status.ModifiedDate,
                    Message = status.Message,
                    ReporterEnum = status.ReporterEnum,
                    StatusEnum = status.StatusEnum,
                    UniqueId = status.UniqueId
                };

                destinationStatuses.Add(destinationStatus);
            }

            return destinationStatuses;
        }

        private IEnumerable<DF_Status> CreateInvalidDestinationStatuses(DF_Status status, IEnumerable<int> invalidDestinations)
        {
            var invalidDestinationStatuses = new List<DF_Status>();

            foreach (var invalidReleasedDestinationId in invalidDestinations)
            {
                var invalidDestinationStatus = new DF_Status
                {
                    AssetID = status.AssetID,
                    DestinationID = invalidReleasedDestinationId,
                    CreatedBy = status.CreatedBy,
                    ModifiedBy = status.ModifiedBy,
                    CreatedDate = status.CreatedDate,
                    ModifiedDate = status.ModifiedDate,
                    Message = string.Concat("This destination might have been mapped to this asset's feeds in the past; ",
                            "however, it is no longer map. Until it is re-mapped or new destination statuses are added, it will be ignored."),
                    ReporterEnum = status.ReporterEnum,
                    StatusEnum = StatusLibrary.GetStatusEnumByValue("IgnoreDestination").Enum,
                    UniqueId = status.UniqueId
                };

                invalidDestinationStatuses.Add(invalidDestinationStatus);
            }

            return invalidDestinationStatuses;
        }

        #endregion


        private static IList<string> GetDestinations(Airing source)
        {
            if (!source.Flights.Any())
                return new List<string>();

            var destinations = new List<string>();

            foreach (var flight in source.Flights)
            {
                destinations.AddRange(flight.Destinations.Select(d => d.Name));
            }

            return destinations.Distinct().ToList();
        }


        private List<Airing> ReleasedAssetByAssetId(string assetId)
        {
            IMongoQuery mongoQuery;
            var criteria = new List<string> { assetId };
            mongoQuery = Query.In("AssetId", new BsonArray(criteria));

            return GetAssetsBy(mongoQuery).ToList();
        }


        public IQueryable<Airing> GetAssetsBy(IMongoQuery query)
        {
            var collection = dbConnection.GetDatabase().GetCollection<Airing>("currentassets");

            var disableTrackingQuery = Query.Or(
                Query.NotExists("DisableTracking"),
                Query.EQ("DisableTracking", false));

            var airings = collection.Find(Query.And(query, disableTrackingQuery));

            return airings.AsQueryable();
        }


        public List<DF_Status> GetStatusesByAssetId(string assetId)
        {
            return dbConnection.GetDatabase().GetCollection<DF_Status>("DFStatus").Find(Query.EQ("AssetID", assetId)).OrderBy(c => c.CreatedDate).ToList();
        }
    }
}
