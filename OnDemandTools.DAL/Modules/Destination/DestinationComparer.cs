using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Destination.Comparer
{
    public class DestinationDataModelComparer : IEqualityComparer<Model.Destination>
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
