using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Product.Queries
{
    public interface IProductQuery
    {
        IQueryable<Model.Product> Get();
        IQueryable<Model.Product> GetByTags(List<string> tags);
        IQueryable<Model.Product> GetByProductIds(List<Guid> productIds);
        Model.Product GetById(string externalId);
        Model.Product GetByMappingId(int mappingId);
    }
}
