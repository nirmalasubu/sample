using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;

namespace OnDemandTools.DAL.Modules.Product.Command
{
    public interface IProductCommand
    {
        DLModel.Product Save(DLModel.Product product);
        void Delete(string id);
    }
}
