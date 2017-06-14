using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Brands.Comparer
{
    public class BrandComparer : IEqualityComparer<Model.Brand>
    {
        public bool Equals(Model.Brand x, Model.Brand y)
        {
            return x.Name == y.Name;
        }

        public bool Equals(Model.Brand x, string y)
        {
            return x.Name == y;
        }

        public int GetHashCode(Model.Brand obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
