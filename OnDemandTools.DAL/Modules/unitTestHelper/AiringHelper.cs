using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.DAL.Modules.unitTestHelper
{
    public class AiringHelper :IAiringHelper
    {
        private readonly MongoDatabase _database;

        private readonly MongoCollection<Airing> _collection;
        public AiringHelper(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
            _collection = _database.GetCollection<Airing>("currentassets");
        }

       public void  UpdateAiringRelesedDate(string airingId, DateTime releasedon)
        {
            var query = Query.EQ("AssetId", airingId);
            var set = Update .Set("ReleaseOn", releasedon);

            _collection.Update(query, set);

            
        }

      
    }
}
