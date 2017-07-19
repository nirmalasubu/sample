using System.Collections.Generic;
using OnDemandTools.Business.Modules.Status.Model;

namespace OnDemandTools.Business.Modules.Status
{
   public interface IStatusSerivce
    {
        /// <summary>
        /// To get all statuses from  status collection
        /// </summary>
        /// <returns>all status</returns>
        IList<Model.Status> GetAllStatus();

        /// <summary>
        /// To delete status 
        /// </summary>
        /// <param name="id">status id</param>
        void Delete(string id);

        /// <summary>
        /// To save status details
        /// </summary>
        /// <param name="status">status</param>
        /// <returns>status model</returns>
        Model.Status Save(Model.Status status);
    }
}
