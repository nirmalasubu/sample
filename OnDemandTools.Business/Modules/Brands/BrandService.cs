﻿using OnDemandTools.DAL.Modules.Brands.Queries;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OnDemandTools.Business.Modules.Brands
{
    public class BrandService : IBrandService
    {
        IBrandQuery _brandQuery;

        public BrandService(IBrandQuery brandQuery)
        {
            _brandQuery = brandQuery;
        }

        public List<string> GetAllBrands()
        {
            return _brandQuery.Get().Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
        }
    }
}
