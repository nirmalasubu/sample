using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLModel = OnDemandTools.Business.Modules.Destination.Model;
using DLModel = OnDemandTools.DAL.Modules.Destination.Model;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.Common.Model;

namespace OnDemandTools.Business.Modules.Destination
{
    /// <summary>
    /// Destination service
    /// </summary>
    /// <seealso cref="OnDemandTools.Business.Modules.Destination.IDestinationService" />
    public class DestinationService : IDestinationService
    {
        IDestinationQuery destinationHelper;

        public DestinationService(IDestinationQuery destinationHelper)
        {
            this.destinationHelper = destinationHelper;
        }


        /// <summary>
        /// Returns first destination that match the given name
        /// </summary>
        /// <param name="destinationName">Name of the destination.</param>
        /// <returns></returns>
        public BLModel.Destination GetByName(string destinationName)
        {
            return
                (destinationHelper.Get().First(d => d.Name == destinationName)               
                .ToBusinessModel<DLModel.Destination, BLModel.Destination>());
        }

        /// <summary>
        /// Gets destinations that matches the given names
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByDestinationNames(List<string> names)
        {
            return (destinationHelper.GetByDestinationNames(names)
                .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }


        /// <summary>
        /// Gets destinations by mapping identifier.
        /// </summary>
        /// <param name="mappingId">The mapping identifier.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByMappingId(int mappingId)
        {
            return (destinationHelper.GetByMappingId(mappingId)
                .ToList<DLModel.Destination>()
                .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        /// <summary>
        /// Gets destinations by product (non unique) identifier. 
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByProductId(Guid productId)
        {
            return (destinationHelper.GetByProductId(productId)
               .ToList<DLModel.Destination>()
               .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        /// <summary>
        /// Gets products by product ids.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public List<BLModel.Destination> GetByProductIds(IList<Guid> productIds)
        {
            return (destinationHelper.GetByProductIds(productIds)
               .ToList<DLModel.Destination>()
               .ToViewModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }

        /// <summary>
        /// Gets all destinations
        /// </summary>
        /// <returns></returns>
        public List<BLModel.Destination> GetAll()
        {
            return
                (destinationHelper.Get().ToList<DLModel.Destination>()
                .ToBusinessModel<List<DLModel.Destination>, List<BLModel.Destination>>());
        }
    }
}
