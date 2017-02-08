using System.Collections.Generic;
using DLModel = OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.Business.Modules.Airing
{
    public class AiringComparer : IEqualityComparer<DLModel.Airing>
    {
        public bool Equals(DLModel.Airing x, DLModel.Airing y)
        {
            return x.AssetId == y.AssetId;
        }

        public int GetHashCode(DLModel.Airing obj)
        {
            return obj.AssetId.GetHashCode();
        }
    }
}