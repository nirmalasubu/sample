﻿using FluentValidation;
using OnDemandTools.Common.Extensions;
using OnDemandTools.DAL.Modules.Airings;
using OnDemandTools.DAL.Modules.Destination.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using BLModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.Airing
{
    public enum AiringValidationRuleSet
    {
        PostAiring,
        DeleteAiring,
        PostPlaylist
    }

    public class AiringValidator : AbstractValidator<BLModel.Airing>
    {
        IGetAiringQuery _airingQuery;
        IDestinationQuery _desQuery;

        public AiringValidator(IGetAiringQuery airingQuery, IDestinationQuery desQuery)
        {
            _airingQuery = airingQuery;
            _desQuery = desQuery;


            RuleSet(AiringValidationRuleSet.PostAiring.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.Network))
                       .WithMessage("Brand required")
                       .Must(c => c.Flights.Any())
                       .WithMessage("Flight information required")
                       .Must(c => !String.IsNullOrEmpty(c.ReleaseBy))
                       .WithMessage("ReleaseBy field required");

                // Detect deleted airing
                RuleFor(c => c.AssetId)
                    .Must(x =>
                    {
                        return (String.IsNullOrEmpty(x)) ? true : !_airingQuery.IsAiringDeleted(x);
                    })
                    .WithMessage("Airing previously deleted. Cannot reuse airing id: {0}", c => c.AssetId);

                // Validate flight information
                RuleForEach(c => c.Flights)
                    .SetValidator(new FlightValidator(_desQuery));
            });



            RuleSet(AiringValidationRuleSet.DeleteAiring.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.AssetId))
                       .WithMessage("Airing Id required")
                       .Must(c => !String.IsNullOrEmpty(c.ReleaseBy))
                       .WithMessage("ReleaseBy field required")
                       .DependentRules(dr =>
                        {
                            // Verify that the given airingId exist                   
                            Func<String, bool> airingIdExistRule = new Func<String, bool>((airingId) =>
                            {
                                try
                                {
                                    return (_airingQuery.GetBy(airingId) != null);
                                }
                                catch (Exception)
                                {
                                    return false;
                                }
                            });

                            dr.RuleFor(c => c.AssetId)
                              .Must(airingIdExistRule)
                              .WithMessage("Provided AiringId does not exist.");
                        });
            });

            RuleSet(AiringValidationRuleSet.PostPlaylist.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => !String.IsNullOrEmpty(c.AssetId))
                       .WithMessage("AiringId is required OR AiringId does not exist.")
                       .Must(c => !String.IsNullOrEmpty(c.ReleaseBy))
                       .WithMessage("ReleaseBy field required")
                        .Must(c => (c.Versions != null && c.Versions.Any()))
                       .WithMessage("Version not exists in existing airing.")
                       .Must(c => (c.PlayList != null && c.PlayList.Any()))
                       .WithMessage("Playlist is required")
                       .DependentRules(pl =>
                         {
                             // Verify that the given airingId exist                   
                             Func<BLModel.Airing, bool> playlistRule = new Func<BLModel.Airing, bool>((airing) =>
                             {
                                 try
                                 {
                                     var foundCids = new List<string>();

                                     foreach (var segment in airing.PlayList.Where(e => e.ItemType == "Segment"))
                                     {
                                         var versionFound = false;
                                         foreach (var version in airing.Versions)
                                         {
                                             if (segment.Id.StartsWith(version.ContentId))
                                             {
                                                 versionFound = true;

                                                 if (!foundCids.Contains(version.ContentId))
                                                     foundCids.Add(version.ContentId);

                                                 break;
                                             }
                                         }

                                         //Return validation error if Segment CID's not matches with Version CID's
                                         if (!versionFound) return false;
                                     }


                                     //Returns true if all version matches with Segements, if not then it will return false.
                                     return foundCids.Count == airing.Versions.Count;
                                 }
                                 catch (Exception)
                                 {
                                     return false;
                                 }
                             });

                             pl.RuleFor(c => c)
                               .Must(playlistRule)
                               .WithMessage("Provided Segment CID(s) does not match with Version CID(s) {0}.",
                               c => string.Join(",", c.Versions.Select(e => e.ContentId)));
                         });
            });
        }
    }



    class FlightValidator : AbstractValidator<BLModel.Flight>
    {
        public FlightValidator(IDestinationQuery desQuery)
        {

            RuleSet(AiringValidationRuleSet.PostAiring.ToString(), () =>
            {
                RuleFor(c => c)               
                .Must(c => !c.Destinations.IsNullOrEmpty() ^ !c.Products.IsNullOrEmpty())
                .WithMessage("Either products or destinations are required, not both");

                RuleForEach(c => c.Products)
                    .SetValidator(new ProductValidator(desQuery));
               
            });
        }
    }


    class ProductValidator : AbstractValidator<BLModel.Product>
    {
        public ProductValidator(IDestinationQuery desQuery)
        {

            RuleSet(AiringValidationRuleSet.PostAiring.ToString(), () =>
            {
                // Verify that product has destination
                RuleFor(c => c)
                     .Must(c =>
                     {
                         return !desQuery
                        .GetByProductIds(new List<Guid> { c.ExternalId })
                        .IsNullOrEmpty();

                     })
                     .WithMessage("Product {0} doesn't have a destination. Products require one or more destination before content can be fulfilled. Please contact ODT team - OnDemandToolsSupport@turner.com", c => c.ExternalId);

            });

        }
    }

  
}