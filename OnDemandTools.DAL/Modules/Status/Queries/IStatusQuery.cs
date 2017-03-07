using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Status.Queries
{
   public interface IStatusQuery
    {
        /// <summary>
        /// To get all statuses from  status collection
        /// </summary>
        /// <returns>all status</returns>
        IList<Model.Status> GetAllStatus();
    }

}
