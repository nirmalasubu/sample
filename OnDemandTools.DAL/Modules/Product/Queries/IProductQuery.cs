using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.Product.Queries
{
    public interface IProductQuery
    {
        IQueryable<Model.Product> Get();
        IQueryable<Model.Product> GetByTags(List<string> tags);
        IQueryable<Model.Product> GetByProductIds(List<Guid> productIds);
    }
}
