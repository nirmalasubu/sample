using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.Destination
{
    public interface IDestinationService
    {
        List<Model.Destination> GetAll();
        Model.Destination GetByName(string destinationName);
        List<Model.Destination> GetByMappingId(int mappingId);
        List<Model.Destination> GetByProductId(Guid productId);
        List<Model.Destination> GetByDestinationNames(List<string> names);
        List<Model.Destination> GetByProductIds(IList<Guid> productIds);
    }
}
