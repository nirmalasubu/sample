using OnDemandTools.DAL.Modules.Reporting.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
    public interface IDfStatusMover
    {
        /// <summary>
        /// Moves the statues to expire status collection from current collection
        /// </summary>
        /// <param name="airingId">the airing to move</param>
        void MoveToExpireCollection(string airingId);

        /// <summary>
        /// Moves the statues to expire status collection from current collection
        /// </summary>
        /// <param name="status">the status to move</param>
        void MoveToExpireCollection(DF_Status status);

        /// <summary>
        /// Moves the statues to current collection from expired collection
        /// </summary>
        /// <param name="airingId"></param>
        void MoveToCurrentCollection(string airingId);

        /// <summary>
        /// Moves the statues to current collection from expired collection
        /// </summary>
        /// <param name="status"></param>
        void MoveToCurrentCollection(DF_Status status);

    }
}