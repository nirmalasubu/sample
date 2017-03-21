namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IDfStatusDeporterService
    {
        /// <summary>
        /// Iterate thru all the DF Statuses and it deports expired airing statuses 
        /// </summary>
        void DeportDfStatuses();

        /// <summary>
        /// Deports the airing status by given airingId
        /// </summary>
        /// <param name="airingId">the airing id</param>
        void DeportByAssetId(string airingId);
    }
}