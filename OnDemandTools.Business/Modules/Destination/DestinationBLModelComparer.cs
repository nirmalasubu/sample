using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Destination
{
    public class DestinationBLModelComparer : IEqualityComparer<Model.Destination>
    {
        public bool Equals(Model.Destination x, Model.Destination y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Model.Destination obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
