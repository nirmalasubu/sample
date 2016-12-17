using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Model.Comparers
{
    public class AiringComparer : IEqualityComparer<Airing>
    {
        public bool Equals(Airing x, Airing y)
        {
            return x.AssetId == y.AssetId;
        }

        public int GetHashCode(Airing obj)
        {
            return obj.AssetId.GetHashCode();
        }
    }
}