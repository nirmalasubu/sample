using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.Airing
{
    public class AiringComparer : IEqualityComparer<BLModel.Airing>
    {
        public bool Equals(BLModel.Airing x, BLModel.Airing y)
        {
            return x.AssetId == y.AssetId;
        }

        public int GetHashCode(BLModel.Airing obj)
        {
            return obj.AssetId.GetHashCode();
        }
    }
}