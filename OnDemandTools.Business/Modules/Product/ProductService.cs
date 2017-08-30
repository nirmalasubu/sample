using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.Destination.Comparer;
using OnDemandTools.DAL.Modules.Destination.Queries;
using OnDemandTools.DAL.Modules.Product.Command;
using OnDemandTools.DAL.Modules.Product.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;
using BLModel = OnDemandTools.Business.Modules.Product.Model;
using DLAiringModel = OnDemandTools.DAL.Modules.Airings.Model;
using DLDestinationModel = OnDemandTools.DAL.Modules.Destination.Model;
using DLModel = OnDemandTools.DAL.Modules.Product.Model;

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
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>());
        }

        /// <summary>
        /// Gets products by matching tags
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        public List<Model.Product> GetByTags(List<string> tags)
        {
            return (productHelper.GetByTags(tags).ToList<DLModel.Product>()
                .ToBusinessModel<List<DLModel.Product>, List<BLModel.Product>>());
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

                AddContentTierToDestinationProperties(destinationData, products);

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

        /// <summary>
        /// Add's Content Tiers to Destination Properties
        /// </summary>
        /// <param name="destinationData"></param>
        /// <param name="products"></param>
        private void AddContentTierToDestinationProperties(List<DLDestinationModel.Destination> destinationData, IQueryable<DLModel.Product> products)
        {
            if (products == null || !products.Any()) return;

            if (destinationData == null || !destinationData.Any()) return;

            foreach (DLModel.Product product in products)
            {
                if (product.ContentTiers != null && product.ContentTiers.Any())
                {
                    foreach (DLModel.ContentTier contentTier in product.ContentTiers)
                    {
                        foreach (DLDestinationModel.Destination des in destinationData)
                        {
                            if (!IsContentTierExistsInDestination(des, contentTier))
                            {
                                DLDestinationModel.Property property = new DLDestinationModel.Property();
                                property.Name = "ContentTier";
                                property.Value = contentTier.Name;
                                property.Brands = contentTier.Brands;
                                property.TitleIds = contentTier.TitleIds;
                                property.SeriesIds = contentTier.SeriesIds;
                                des.Properties.Add(property);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Checks the given content tier already added to Destination property or not.
        /// </summary>
        /// <param name="des"></param>
        /// <param name="contentTier"></param>
        /// <returns></returns>
        private bool IsContentTierExistsInDestination(DLDestinationModel.Destination des, DLModel.ContentTier contentTier)
        {
            DLDestinationModel.Property destProperty = des.Properties.FirstOrDefault(e => e.Name == "ContentTier" && e.Value == contentTier.Name);

            if (destProperty == null) return false;

            bool brandsAreEquivalent = (destProperty.Brands.Count == destProperty.Brands.Count)
                && !destProperty.Brands.Except(destProperty.Brands).Any();

            bool titlesAreEquivalent = (destProperty.TitleIds.Count == destProperty.TitleIds.Count)
                && !destProperty.TitleIds.Except(destProperty.TitleIds).Any();

            bool seriesAreEquivalent = (destProperty.SeriesIds.Count == destProperty.SeriesIds.Count)
                && !destProperty.SeriesIds.Except(destProperty.SeriesIds).Any();

            return (brandsAreEquivalent && titlesAreEquivalent && seriesAreEquivalent);
        }

        /// <summary>
        /// Add's the Categories to Destination Properties
        /// </summary>
        /// <param name="destinationData"></param>
        private static void AddCategoriesToDestinationProperties(List<DLDestinationModel.Destination> destinationData)
        {
            foreach (DLDestinationModel.Destination des in destinationData)  //verify each destination has categories . if yes then combine categories and properties.
            {
                if (des.Categories.Any())
                {
                    foreach (DLDestinationModel.Category cat in des.Categories)
                    {
                        DLDestinationModel.Property property = new DLDestinationModel.Property();
                        property.Name = "Category";
                        property.Value = cat.Name;
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


        /// <summary>
        /// Get's the product by id
        /// </summary>
        /// <param name="externalId"> id of the product</param>
        /// <returns></returns>
        public BLModel.Product GetById(string externalId)
        {
            return (productHelper.GetById(externalId)
               .ToBusinessModel<DLModel.Product, BLModel.Product>());
        }

        /// <summary>
        /// Get's the product by mapping id
        /// </summary>
        /// <param name="mappingId"> id of the product</param>
        /// <returns></returns>
        public BLModel.Product GetByMappingId(int mappingId)
        {
            return (productHelper.GetByMappingId(mappingId)
               .ToBusinessModel<DLModel.Product, BLModel.Product>());
        }

        /// <summary>
        /// Delete's content tier by name
        /// </summary>
        /// <param name="contentTierName"></param>
        public void DeleteContentTierByName(string contentTierName)
        {
            productCommand.DeleteContentTierByName(contentTierName);
        }
    }
}
