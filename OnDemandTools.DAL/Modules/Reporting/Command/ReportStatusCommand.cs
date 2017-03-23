using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using OnDemandTools.DAL.Modules.Reporting.Model;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Reporting.Library;
using OnDemandTools.DAL.Modules.Reporting.Queries;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Airings.Model.Comparers;
using MongoDB.Bson;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
    public class ReportStatusCommand : IReportStatusCommand
    {

        DestinationLibrary q;

        private const int BIMFOUND = 17;
        private const int BIMNOTFOUND = 18;
        private const int BIMMISMATCH = 19;
        private readonly MongoDatabase _database;
        readonly AppSettings _configuration;

        public ReportStatusCommand(IODTDatastore connection, AppSettings configuration)
        {
            _database = connection.GetDatabase();
            this._configuration = configuration;
            q = new DestinationLibrary(configuration);
        }


        #region PUBLIC METHODS
        public void Report(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, string message = "", bool unique = false)
        {
            SaveStatus(airingId, isActiveAiringStatus, statusEnum, destinationEnum, unique, message);

        }

        public void Report(string airingId, bool isActiveAiringStatus, string statusMessage, int dfStatus = 13, int dfDestination = 18)
        {
            Report(airingId, isActiveAiringStatus, dfStatus, dfDestination, statusMessage, false);
        }

        /// <summary>
        /// Report the bim status message to Df
        /// </summary>
        /// <param name="airingId">airingId</param>
        /// <param name="statusEnum">statusEnum for BIM is 18,17,4</param>
        /// <param name="isActiveAiringStatus">is active airing?</param>
        /// <param name="destinationEnum">the destination enum</param>
        /// <param name="message">the message</param>
        public void BimReport(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, string message = "")
        {
            SaveBIMStatus(airingId, isActiveAiringStatus, statusEnum, destinationEnum, message);

        }

        public void Report(Airing airing, bool isActiveAiringStatus)
        {
            foreach (var destination in airing.Flights.SelectMany(f => f.Destinations.Distinct(new DestinationComparer())))
            {
                Report(airing.AssetId, isActiveAiringStatus, 14, destination.ExternalId);
            }
        }
        #endregion

        #region "Throw away code once new Auditing system is online"
        private void SaveStatus(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, bool unique, string message)
        {
            var status = new DF_Status
            {
                AssetID = airingId,
                StatusEnum = statusEnum,
                Message = message,
                CreatedBy = "OnDemandTools",
                ReporterEnum = 9, //OnDemandTools
                DestinationID = destinationEnum
            };

            var dataModel = CreateStatuses(new List<DF_Status> { status }, isActiveAiringStatus, unique);
        }

        public List<DF_Status> CreateStatuses(List<DF_Status> statusDataModels, bool isActiveAiringStatus, bool unique)
        {
            var results = new List<DF_Status>();
            try
            {
                statusDataModels.ForEach(status =>
                {
                    status.StatusID = 0;

                    if (status.DestinationID == q.GetByName("NONE").DestinationID && status.StatusEnum == StatusLibrary.GetStatusEnumByValue("Released").Enum)
                    {
                        var releasedStatusMapper = new ReleasedStatusToDestinationMapper(_configuration);
                        var releasedDestinationStatuses = releasedStatusMapper.CreateDestinationStatuses(status, q.GetDestinations(), isActiveAiringStatus);

                        foreach (var releasedDestiantionStatus in releasedDestinationStatuses)
                        {
                            results.Add(unique ? CreateUniqueStatus(releasedDestiantionStatus, isActiveAiringStatus) : CreateStatus(releasedDestiantionStatus, isActiveAiringStatus));
                        }
                    }
                    else
                    {
                        results.Add(unique ? CreateUniqueStatus(status, isActiveAiringStatus) : CreateStatus(status, isActiveAiringStatus));
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }

            return results;
        }
        #endregion

        #region  SAVE STATUS IN DF_STATUS COLLECTION
        public DF_Status CreateUniqueStatus(DF_Status status, bool isActiveAiringStatus)
        {
            return !DoesStatusExistForAssetReporterAndDestination(status, isActiveAiringStatus) ? CreateStatus(status, isActiveAiringStatus) : new DF_Status
            {
                AssetID = status.AssetID,
                StatusEnum = status.StatusEnum,
                CreatedBy = status.CreatedBy,
                CreatedDate = DateTime.Now,
                DestinationID = q.GetByName("None").DestinationID,
                ModifiedBy = status.ModifiedBy,
                ModifiedDate = DateTime.Now,
                ReporterEnum = status.ReporterEnum,
                Message = "Status for specfied asset and destination aready exists and will not be created to avoid duplicates."
            };
        }

        private bool DoesStatusExistForAssetReporterAndDestination(DF_Status status, bool isActiveAiringStatus)
        {

            ODTDatastore _dbODT = new ODTDatastore(_configuration);
            var statusCollection = _dbODT.GetDatabase().GetCollection<DF_Status>(isActiveAiringStatus ? "DFStatus" : "DFExpiredStatus");
            var query = Query.And(Query.EQ("AssetID", status.AssetID),
                                  Query.EQ("StatusEnum", status.StatusEnum),
                                  Query.EQ("ReporterEnum", status.ReporterEnum),
                                  Query.EQ("DestinationID", status.DestinationID));
            return statusCollection.Find(query).Any();
        }

        public DF_Status CreateStatus(DF_Status status, bool isActiveAiringStatus)
        {
            try
            {
                ODTDatastore _dbODT = new ODTDatastore(_configuration);
                int highOrder = _dbODT.GetDatabase().GetCollection<DF_Status>(isActiveAiringStatus ? "DFStatus" : "DFExpiredStatus").FindAll()
                    .SetSortOrder(SortBy.Descending(new[] { "StatusID" }))
                    .SetFields(new[] { "StatusID" })
                    .SetLimit(1)
                    .FirstOrDefault().StatusID;

                status.StatusID = ++highOrder;
                status.CreatedDate = DateTime.Now;
                status.ModifiedDate = status.CreatedDate;
                status.ModifiedBy = status.CreatedBy;
                MongoCollection<DF_Status> statusCollection = _dbODT.GetDatabase().GetCollection<DF_Status>(isActiveAiringStatus ? "DFStatus" : "DFExpiredStatus");
                statusCollection.Save(status);
            }
            catch (Exception)
            {
                throw;
            }

            return status;
        }
        #endregion

        #region SAVE BIM STATUS IN DF_STATUS COLLECTION
        private void SaveBIMStatus(string airingId, bool isActiveAiringStatus, int statusEnum, int destinationEnum, string message)
        {
            var status = new DF_Status
            {
                AssetID = airingId,
                StatusEnum = statusEnum,
                Message = message,
                CreatedBy = "OnDemandTools",
                ReporterEnum = 9, //OnDemandTools
                DestinationID = destinationEnum
            };

            UpdateStatus(status, isActiveAiringStatus);
        }

        private void UpdateStatus(DF_Status status, bool isActiveAiringStatus)
        {
            var bimStatus = new BsonValue[] { BIMFOUND, BIMMISMATCH, BIMNOTFOUND };
            var dfStatusCollection = _database.GetCollection<DF_Status>(isActiveAiringStatus ? "DFStatus" : "DFExpiredStatus");

            var query = Query.And(Query.EQ("AssetID", status.AssetID),
                                Query.In("StatusEnum", bimStatus),
                                Query.EQ("ReporterEnum", status.ReporterEnum),
                                Query.EQ("DestinationID", status.DestinationID));
            bool isStatusExistsinDFStatus = dfStatusCollection.Find(query).Any();

            int highOrder = dfStatusCollection.FindAll()
         .SetSortOrder(SortBy.Descending(new[] { "StatusID" }))
         .SetFields(new[] { "StatusID" })
         .SetLimit(1)
         .FirstOrDefault().StatusID;
            status.StatusID = ++highOrder;
            status.CreatedDate = DateTime.Now;
            status.ModifiedDate = status.CreatedDate;
            status.ModifiedBy = status.CreatedBy;
            if (!isStatusExistsinDFStatus)
            {
                dfStatusCollection.Save(status);

            }
            else
            {

                dfStatusCollection.Update(query,
                Update<DF_Status>.Set(c => c.StatusID, status.StatusID)
                .Set(c => c.StatusEnum, status.StatusEnum)
                .Set(c => c.Message, status.Message)
                .Set(c => c.CreatedDate, status.CreatedDate)
                .Set(c => c.ModifiedDate, status.ModifiedDate)
                .Set(c => c.ModifiedBy, status.ModifiedBy)
                );
            }
        }
        #endregion
    }

}
