using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Destination.Queries
{
    public interface IDestinationQuery
    {
        IQueryable<Model.Destination> Get();
        IQueryable<Model.Destination> GetByMappingId(int mappingId);
        IQueryable<Model.Destination> GetByProductId(Guid productId);
        List<Model.Destination> GetByDestinationNames(List<string> names);
        IQueryable<Model.Destination> GetByProductIds(IList<Guid> productIds);
    }

}
