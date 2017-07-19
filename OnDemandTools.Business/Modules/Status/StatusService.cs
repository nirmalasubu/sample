
using System.Collections.Generic;

using OnDemandTools.DAL.Modules.Status.Queries;
using OnDemandTools.Common.Model;
using BLModel = OnDemandTools.Business.Modules.Status.Model;
using DLModel = OnDemandTools.DAL.Modules.Status.Model;
using System;
using OnDemandTools.DAL.Modules.Status.Command;

namespace OnDemandTools.Business.Modules.Status
{
    public class StatusService : IStatusSerivce
    {
         IStatusQuery _statusQuery;
        IStatusCommand _statusCommand;

        #region CONSTRUCTOR
        public StatusService(IStatusQuery statusQuery, IStatusCommand statusCommand)
        {
            _statusQuery = statusQuery;
            _statusCommand = statusCommand;
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

        /// <summary>
        /// To delete status 
        /// </summary>
        /// <param name="id">status id</param>
        public void Delete(string id)
        {
            _statusCommand.Delete(id);
        }

        /// <summary>
        /// To save status details
        /// </summary>
        /// <param name="status">status</param>
        /// <returns>status model</returns>
        public BLModel.Status Save(BLModel.Status status)
        {
            DLModel.Status dataModel = status.ToDataModel<BLModel.Status, DLModel.Status>();

            dataModel = _statusCommand.Save(dataModel);

            return dataModel.ToBusinessModel<DLModel.Status, BLModel.Status>(); ;
        }
        #endregion
    }
}
