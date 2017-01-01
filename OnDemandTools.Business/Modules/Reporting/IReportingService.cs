using BLAiringModel = OnDemandTools.Business.Modules.Airing.Model;

namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IReportingService
    {
        /// <summary>
        /// Reports a status of 'Note' (enum = 13), along with the provided
        /// status message to reporting system. Status will be applied to the provided
        /// airing identifier.
        /// </summary>
        /// <param name="airingId">The airing identifier.</param>
        /// <param name="statusMessage">The status message.</param>
        /// <param name="dfStatus">The df status.</param>
        /// <param name="dfDestination">The df destination.</param>
        void Report(string airingId, string statusMessage, int dfStatus = 13, int dfDestination = 18);


        /// <summary>
        /// Reports status along with the provided
        /// status message to reporting system. Status will be applied to the provided
        /// airing identifier.
        /// </summary>
        /// <param name="airingId">The airing identifier.</param>
        /// <param name="statusEnum">The status enum.</param>
        /// <param name="destinationEnum">The destination enum.</param>
        /// <param name="message">The message.</param>
        /// <param name="unique">if set to <c>true</c> [unique].</param>
        void Report(string airingId, int statusEnum, int destinationEnum, string message, bool unique = false);

        /// <summary>
        /// Reports the specified airing.
        /// </summary>
        /// <param name="airing">The airing.</param>
        void Report(BLAiringModel.Airing airing);
    }
}
