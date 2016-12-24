using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Queue.Queries
{
    public interface IQueueQuery
    {
        IQueryable<Model.Queue> GetByStatus(bool active);
        IQueryable<Model.Queue> Get();

        Model.Queue Get(int id);

        Model.Queue GetByApiKey(string apiKey);

        IQueryable<Model.Queue> GetPackageQueues();
    }
}
