using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System.Collections.Generic;
using System.Linq;


namespace OnDemandTools.DAL.Modules.Status.Queries
{
    public class StatusQuery:IStatusQuery
    {
        private readonly MongoCollection<Model.Status> _statusCollection;

        #region CONSTRUCTOR
        public StatusQuery(IODTDatastore connection)
        {
            _statusCollection = connection.GetDatabase().GetCollection<Model.Status>("airingstatus");
        }
        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// To get all statuses from  status collection
        /// </summary>
        /// <returns>all status</returns>
        public IList<Model.Status> GetAllStatus()
        {
            return _statusCollection.AsQueryable().ToList();
        }
        #endregion  
    }
}
