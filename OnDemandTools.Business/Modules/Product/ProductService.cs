using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Product.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Product.Model;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;
using OnDemandTools.DAL.Modules.Destination.Comparer;
using OnDemandTools.DAL.Modules.Destination.Queries;

using DLDestinationModel = OnDemandTools.DAL.Modules.Destination.Model;
using DLAiringModel = OnDemandTools.DAL.Modules.Airings.Model;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.DAL.Modules.Product.Command;

namespace OnDemandTools.Business.Modules.Product
{
    public class ProductService : IProductService
    {
        IProductQuery productHelper;
        IProductCommand productCommand;
        IDestinationQuery destionationQueryHelper;

        public ProductService(IProductQuery productHelper, 
            IDestinationQuery destionationQueryHelper,
            IProductCommand productCommand
            )
        {
            this.productHelper = productHelper;
            this.destionationQueryHelper = destionationQueryHelper;
            this.productCommand = productCommand;
        }


        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        public List<BLModel.Product> GetAll()
        {
            return (productHelper.Get().ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>());
        }

        /// <summary>
        /// Gets products by matching product ids
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        public List<Model.Product> GetByProductIds(List<Guid> productIds)
        {
            return (productHelper.GetByProductIds(productIds).ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>()) ;
        }

        /// <summary>
        /// Gets products by matching tags
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        public List<Model.Product> GetByTags(List<string> tags)
        {         
            return (productHelper.GetByTags(tags).ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>,List<BLModel.Product>>());
        }

        public void ProductDestinationConverter(ref Airing.Model.Airing airing)
        {
            foreach (var flight in airing.Flights)
            {
                var products = productHelper.GetByProductIds(flight.Products.Select(p => p.ExternalId).ToList());
                var destinationMapping = products.SelectMany(product => product.Destinations, (product, destination) => new
                {
                    DestinationName = destination,
                    AuthorizationRequired = flight.Products.Where(p => p.ExternalId.Equals(product.ExternalId)).FirstOrDefault().IsAuth
                });
                var destinationData = destionationQueryHelper
                    .GetByDestinationNames(destinationMapping.Select(d => d.DestinationName).ToList())
                    .Distinct(new DestinationDataModelComparer())
                    .ToList();

                AddCategoriesToDestinationProperties(destinationData);

                var destinations = destinationData.ToDataModel<List<DLDestinationModel.Destination>, List<DLAiringModel.Destination>>();

                foreach (var destination in destinations)
                {
                    //if there are multiple destinations with the same name, the least restrictive (isAuth=false) will win
                    if (destinationMapping.Where(dm => dm.DestinationName.Equals(destination.Name)).Count() > 1 && destinationMapping.Any(dm => dm.DestinationName.Equals(destination.Name) && !dm.AuthorizationRequired))
                        destination.AuthenticationRequired = false;
                    else destination.AuthenticationRequired = destinationMapping.Where(dm => dm.DestinationName.Equals(destination.Name)).FirstOrDefault().AuthorizationRequired;
                }

                flight.Destinations = destinations.ToBusinessModel<List<DLAiringModel.Destination>, List<BLAiringModel.Destination>>();


            }
        }


        private static void AddCategoriesToDestinationProperties(List<DLDestinationModel.Destination> destinationData)
        {
            foreach (DLDestinationModel.Destination des in destinationData)  //verify each destination has categories . if yes then combine categories and properties.
            {
                if (des.Categories.Any())
                {
                    foreach (DLDestinationModel.Category cat in des.Categories)
                    {
                        DLDestinationModel.Property property = new DLDestinationModel.Property();
                        property.Name = cat.Name;
                        property.Brands = cat.Brands;
                        property.TitleIds = cat.TitleIds;
                        property.SeriesIds = cat.SeriesIds;
                        des.Properties.Add(property);
                    }
                }
            }
        }

        /// <summary>
        /// Save product to collection
        /// </summary>
        /// <param name="model">product model</param>
        public BLModel.Product Save(BLModel.Product model)
        {
            DLModel.Product dataModel = model.ToDataModel<BLModel.Product, DLModel.Product>();

            dataModel = productCommand.Save(dataModel);

            return dataModel.ToBusinessModel<DLModel.Product, BLModel.Product>();
        }

        /// <summary>
        /// Remove product from collection using ObjectID
        /// </summary>
        /// <param name="id">Object Id</param>
        public void Delete(string id)
        {
            productCommand.Delete(id);
        }
    }
}
