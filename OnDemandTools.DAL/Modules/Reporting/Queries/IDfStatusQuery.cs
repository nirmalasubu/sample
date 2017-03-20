using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.DAL.Modules.Reporting.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Queries
{
    public interface IDfStatusQuery
    {
        IQueryable<DF_Status> GetDfStatusEnumByModifiedDate(DateTime modifedTime);
    }
}
