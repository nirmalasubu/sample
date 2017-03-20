using OnDemandTools.DAL.Modules.Reporting.Model;

namespace OnDemandTools.DAL.Modules.Reporting.Command
{
    public interface IDfStatusMover
    {
        void MoveToExpireCollection(DF_Status status);
    }
}