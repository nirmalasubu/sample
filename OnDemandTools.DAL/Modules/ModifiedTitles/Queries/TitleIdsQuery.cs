using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.ModifiedTitles.Commands;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.ModifiedTitles.Queries
{
    public class TitleIdsQuery : ITitleIdsQuery
    {
        private readonly MongoCollection<ModifiedTitle> _modifiedTitle;
        
        public TitleIdsQuery(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _modifiedTitle = database.GetCollection<ModifiedTitle>("ModifiedTitle");
        }

        public IEnumerable<int> Get(int limit)
        {
            var titleIds = _modifiedTitle.FindAll().SetLimit(limit);

            return titleIds.Select(t => t.TitleId);
        }
    }
}