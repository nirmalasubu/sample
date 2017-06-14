using System.Linq;

namespace OnDemandTools.DAL.Modules.Brands.Queries
{
    public interface INameBrandQuery
    {
        IQueryable<string> Get();
    }

    public interface IGetBrandByNameQuery 
    {
        Model.Brand GetBy(string name);
    }
}
