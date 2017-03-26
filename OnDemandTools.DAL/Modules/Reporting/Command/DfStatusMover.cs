using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Reporting.Model;
using OnDemandTools.DAL.Modules.Reporting.Queries;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
    public class DfStatusMover : IDfStatusMover
    {
        private readonly MongoCollection<DF_Status> _currentCollection;

        private readonly MongoCollection<DF_Status> _expiredCollection;

        private readonly IDfStatusQuery _statusQuery;

        public DfStatusMover(IODTDatastore connection, IDfStatusQuery statusQuery)
        {
            var database = connection.GetDatabase();
            _statusQuery = statusQuery;
            _currentCollection = database.GetCollection<DF_Status>("DFStatus");
            _expiredCollection = database.GetCollection<DF_Status>("DFExpiredStatus");
        }


        /// <summary>
        /// Deport's associated airing statuses by airingId
        /// </summary>
        /// <param name="airingid">the airing id</param>
        public void MoveToExpireCollection(string airingid)
        {
            foreach (var dfStatus in _statusQuery.GetDfStatuses(airingid, true))
            {
                MoveToExpireCollection(dfStatus);
            }
        }

        /// <summary>
        /// Deport's associated airing statuses by airingId
        /// </summary>
        /// <param name="airingid">the airing id</param>
        public void MoveToCurrentCollection(string airingid)
        {
            foreach (var dfStatus in _statusQuery.GetDfStatuses(airingid, false))
            {
                MoveToExpireCollection(dfStatus);
            }
        }

        /// <summary>
        /// Deports the Status to Expired Collection
        /// </summary>
        /// <param name="status"></param>
        public void MoveToExpireCollection(DF_Status status)
        {
            if (_expiredCollection.FindOne(Query<DF_Status>.EQ(e => e.Id, status.Id)) == null)
            {
                _expiredCollection.Save(status);
            }

            var query = Query<DF_Status>.EQ(e => e.Id, status.Id);

            _currentCollection.Remove(query);
        }

        /// <summary>
        /// Deports the Status to Expired Collection
        /// </summary>
        /// <param name="status"></param>
        public void MoveToCurrentCollection(DF_Status status)
        {
            var expiredStatus = CloneStatus(status);
            _currentCollection.Save(expiredStatus);

            var query = Query<DF_Status>.EQ(e => e.Id, status.Id);

            _expiredCollection.Remove(query);
        }



        /// <summary>
        /// Clones the status
        /// </summary>
        /// <param name="status">the DF status to clone</param>
        /// <returns></returns>
        private DF_Status CloneStatus(DF_Status status)
        {
            var clonedStatusString = JsonConvert.SerializeObject(status);
            var clonedStatus = JsonConvert.DeserializeObject<DF_Status>(clonedStatusString);
            clonedStatus.Id = string.Empty;
            return clonedStatus;
        }
    }
}