using System.Collections.Generic;
using System.Linq;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating
{
    public class AiringValidator
    {
        private readonly List<IAiringValidatorStep> _validatorSteps;

        public AiringValidator(List<IAiringValidatorStep> validatorSteps)
        {
            _validatorSteps = validatorSteps;
        }

        public IList<ValidationResult> Validate(Airing airing, string remoteQueueName)
        {
            var results = new List<ValidationResult>();

            foreach (var step in _validatorSteps)
            {
                results.Add(step.Validate(airing, remoteQueueName));
            }

            return results;
        }
    }


    /// <summary>
    /// Verify that the airing has versions.
    /// </summary>
    /// <seealso cref="IAiringValidatorStep" />
    public class VersionValidator : IAiringValidatorStep
    {

        /// <summary>
        /// Validates the specified airing.
        /// </summary>
        /// <param name="airing">The airing.</param>
        /// <returns></returns>
        ValidationResult IAiringValidatorStep.Validate(Airing airing, string remoteQueueName = "")
        {
            // Verify that version exist
            if (!airing.Versions.Any())
                return new ValidationResult(false, 11, "Version is missing when it was required.", true);

            // Return true if all validation passes
            return new ValidationResult(true);
        }
    }
}