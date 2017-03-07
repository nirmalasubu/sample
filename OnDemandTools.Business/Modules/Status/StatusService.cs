
using System.Collections.Generic;

using OnDemandTools.DAL.Modules.Status.Queries;
using OnDemandTools.Common.Model;
using BLModel = OnDemandTools.Business.Modules.Status.Model;
using DLModel = OnDemandTools.DAL.Modules.Status.Model;

namespace OnDemandTools.Business.Modules.Status
{
    public class StatusService : IStatusSerivce
    {
         IStatusQuery _statusQuery;

        #region CONSTRUCTOR
        public StatusService(IStatusQuery statusQuery)
        {
            _statusQuery = statusQuery;
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// To get all statuses from  status collection
        /// </summary>
        /// <returns>all status</returns>
        public IList<Model.Status> GetAllStatus()
        {
            return _statusQuery.GetAllStatus().ToBusinessModel<IList<DLModel.Status>, IList<BLModel.Status>>();
        }
        #endregion
    }
}
