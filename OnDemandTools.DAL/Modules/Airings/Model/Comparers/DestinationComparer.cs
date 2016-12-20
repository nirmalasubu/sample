using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Model.Comparers
{
    public class DestinationComparer : IEqualityComparer<Destination>
    {
        public bool Equals(Destination x, Destination y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Destination obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}