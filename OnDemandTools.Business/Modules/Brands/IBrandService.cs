using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Brands
{
    public interface IBrandService
    {
        List<string> GetAllBrands();
    }
}
