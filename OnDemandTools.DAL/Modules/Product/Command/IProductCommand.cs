using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Product.Command
{
    public interface IProductCommand
    {
        void Delete(string id);
    }
}
