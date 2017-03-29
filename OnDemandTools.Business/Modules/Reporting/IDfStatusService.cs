namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IDfStatusService
    {
        /// <summary>
        /// Checks the airing DF messages exists in Current or Expired DF Status collection
        /// </summary>
        /// <param name="airingId">the airing id</param>
        /// <param name="isActiveAiringCollection">is Active Airing Collection?</param>
        /// <returns></returns>
        bool HasMessages(string airingId, bool isActiveAiringCollection);
    }
}