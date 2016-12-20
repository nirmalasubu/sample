using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;

namespace OnDemandTools.DAL.Modules.Airings.Queries
{
    public class CurrentAiringsQuery : AiringQuery, IAiringQuery
    {
        public CurrentAiringsQuery(IODTDatastore connection)
            : base(connection.GetDatabase().GetCollection<Airing>("currentassets"))
        {

        }

        public Airing GetExampleBy(string jsonQuery)
        {
             var queryDoc = Create(jsonQuery);

            var airing = Collection.FindOneAs<Airing>(queryDoc);

            return airing;
        }
    }
}