using FluentValidation;
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
        public PackageValidator()
        {
            RuleSet(PackageValidatorRuleSet.PostPackage.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => c.TitleIds.Any())
                       .WithMessage("At least one TitleId is required")
                       .Must(c => !string.IsNullOrEmpty(c.Type))
                       .WithMessage("Type field must be provided")
                       .Must(c => (c.PackageData != null))
                       .WithMessage("PackageData must contain valid JSON")
                       .Must(c => (c.PackageData != null && !string.IsNullOrEmpty(c.PackageData.ToString()) && !c.PackageData.ToString().Equals("{}")))
                       .WithMessage("PackageData cannot be empty");
            });

            RuleSet(PackageValidatorRuleSet.DeletePackage.ToString(), () =>
            {
                // Verify required fields are provided
                RuleFor(c => c)
                       .Must(c => c.TitleIds.Any())
                       .WithMessage("At least one TitleId is required")
                       .Must(c => !string.IsNullOrEmpty(c.Type))
                       .WithMessage("Type field must be provided");
            });
        }
    }
}
