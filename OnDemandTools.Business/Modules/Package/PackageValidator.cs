using FluentValidation;
using OnDemandTools.Common.Extensions;
using OnDemandTools.DAL.Modules.Airings;
using System;
using System.Linq;

namespace OnDemandTools.Business.Modules.Package
{
    public enum PackageValidatorRuleSet
    {
        PostPackage,
        DeletePackage
    }

    public class PackageValidator : AbstractValidator<Model.Package>
    {
        IGetAiringQuery _airingQuery;
        public PackageValidator(IGetAiringQuery airingQuery)
        {
            _airingQuery = airingQuery;

            RuleSet(PackageValidatorRuleSet.PostPackage.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => (!string.IsNullOrEmpty(c.AiringId) || c.TitleIds.Any() || (c.ContentIds.Any()&& !c.ContentIds.All(x => string.IsNullOrEmpty(x)))))
                       .WithMessage("At least one AiringId or  TitleId or ContentId is required")
                       .Must(c => !(c.ContentIds.Any() && c.TitleIds.Any() && string.IsNullOrEmpty(c.AiringId))
                                   && !(!string.IsNullOrEmpty(c.AiringId) && c.ContentIds.Any() && c.TitleIds.Any())
                                   && !(!string.IsNullOrEmpty(c.AiringId) && c.ContentIds.Any() && !c.TitleIds.Any())
                                   && !(!string.IsNullOrEmpty(c.AiringId) && c.TitleIds.Any() && !c.ContentIds.Any()))
                       .WithMessage("Cannot register package. Must only provide either AiringId or TitleId or ContentId")
                       .Must(c => !string.IsNullOrEmpty(c.Type))
                       .WithMessage("Type field must be provided")
                       .Must(c => (c.PackageData != null))
                       .WithMessage("PackageData must contain valid JSON")
                       .Must(c => (c.PackageData != null && !string.IsNullOrEmpty(c.PackageData.ToString()) && !c.PackageData.ToString().Equals("{}")))
                       .WithMessage("PackageData cannot be empty")
                       .DependentRules(dr =>
                       {
                           // Verify that the given airingId exist                   
                           Func<String, bool> airingIdExistRule = new Func<String, bool>((airingId) =>
                           {
                               try
                               {
                                   return (string.IsNullOrEmpty(airingId)) ? true : (_airingQuery.GetBy(airingId) != null);
                               }
                               catch (Exception)
                               {
                                   return false;
                               }
                           });

                           dr.RuleFor(c => c.AiringId)
                             .Must(airingIdExistRule)
                             .WithMessage("Provided AiringId does not exist.");
                       });


            });

            RuleSet(PackageValidatorRuleSet.DeletePackage.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => (!string.IsNullOrEmpty(c.AiringId) || c.TitleIds.Any() || c.ContentIds.Any()))
                       .WithMessage("At least one AiringId or  TitleId or ContentId is required")
                       .Must(c => !(c.ContentIds.Any() && c.TitleIds.Any() && string.IsNullOrEmpty(c.AiringId))
                                   && !(!string.IsNullOrEmpty(c.AiringId) && c.ContentIds.Any() && c.TitleIds.Any())
                                   && !(!string.IsNullOrEmpty(c.AiringId) && c.ContentIds.Any() && !c.TitleIds.Any())
                                   && !(!string.IsNullOrEmpty(c.AiringId) && c.TitleIds.Any() && !c.ContentIds.Any()))
                       .WithMessage("Cannot delete package. Must only provide either AiringId or TitleId or ContentId")
                       .Must(c => !string.IsNullOrEmpty(c.Type))
                       .WithMessage("Type field must be provided")
                       .DependentRules(dr =>
                       {
                           // Verify that the given airingId exist                   
                           Func<String, bool> airingIdExistRule = new Func<String, bool>((airingId) =>
                           {
                               try
                               {
                                   return (string.IsNullOrEmpty(airingId)) ? true : (_airingQuery.GetBy(airingId) != null);
                               }
                               catch (Exception)
                               {
                                   return false;
                               }
                           });

                           dr.RuleFor(c => c.AiringId)
                             .Must(airingIdExistRule)
                             .WithMessage("Provided AiringId does not exist.");
                       });
            });
        }
    }
}
