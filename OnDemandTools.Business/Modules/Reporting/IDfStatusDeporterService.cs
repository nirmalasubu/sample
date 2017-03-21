namespace OnDemandTools.Business.Modules.Reporting
{
    public interface IDfStatusDeporterService
    {
        /// <summary>
        /// Iterate thru all the DF Statuses and it deports expired airing statuses 
        /// </summary>
        void DeportDfStatuses();
    }
}