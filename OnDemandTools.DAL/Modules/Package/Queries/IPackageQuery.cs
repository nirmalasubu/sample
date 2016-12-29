using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Package.Queries
{
    public interface IPackageQuery
    {
        Model.Package GetBy(List<int> titleIds, string destinationCode, string type);
        List<Model.Package> GetBy(List<int> titleIds, IList<string> destinationCodes);
    }

}
