using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Reporting.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
    public class DfStatusMover : IDfStatusMover
    {
        private readonly MongoCollection<DF_Status> _currentCollection;

        private readonly MongoCollection<DF_Status> _expiredCollection;

        public DfStatusMover(IODTDatastore connection)
        {
            var database = connection.GetDatabase();
            _currentCollection = database.GetCollection<DF_Status>("DFStatus");
            _expiredCollection = database.GetCollection<DF_Status>("DFExpiredStatus");
        }

        public void MoveToExpireCollection(DF_Status status)
        {
            _expiredCollection.Save(status);

            var query = Query<DF_Status>.EQ(e => e.Id, status.Id);

            _currentCollection.Remove(query);
        }
    }
}