namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IDfStatusDeporterService
    {
        /// <summary>
        /// Iterate thru all the DF Statuses and it deports expired airing statuses 
        /// </summary>
        void DeportDfStatuses();

        /// <summary>
        /// Checks the airing DF messages exists in Current or Expired DF Status collection
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="isActiveAiringCollection">is Active Airing Collection?</param>
        /// <returns></returns>
        bool HasMessages(string airingId, bool isActiveAiringCollection);
    }
}