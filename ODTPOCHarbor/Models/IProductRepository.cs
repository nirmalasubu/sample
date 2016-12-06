using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTPOCHarbor.Models
{
    public interface IProductRepository
    {

        void Add(Product item);
        IEnumerable<Product> GetAll();
    }
}
