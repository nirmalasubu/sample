using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Brands.Command
{
    public interface IBrandSaveCommand
    {
        void Save(List<Model.Brand> brands);
    }
}
