using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class DeletedAiringsQuery : AiringQuery, IDeletedAiringsQuery
    {
        public DeletedAiringsQuery(IODTDatastore connection)
            : base(connection.GetDatabase().GetCollection<Airing>("deletedasset"))
        {

        }
    }
}