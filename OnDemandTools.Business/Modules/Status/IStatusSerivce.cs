using System.Collections.Generic;


namespace OnDemandTools.Business.Modules.Status
{
   public interface IStatusSerivce
    {
        /// <summary>
        /// To get all statuses from  status collection
        /// </summary>
        /// <returns>all status</returns>
        IList<Model.Status> GetAllStatus();
    }
}
