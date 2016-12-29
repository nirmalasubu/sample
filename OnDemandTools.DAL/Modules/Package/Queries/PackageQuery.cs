using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;

namespace OnDemandTools.DAL.Modules.Package.Queries
{
    public class PackageQuery : IPackageQuery
    {
        private readonly MongoDatabase _database;

        public PackageQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public List<Model.Package> GetBy(List<int> titleIds, IList<string> destinationCodes)
        {
            var collection = _database
                .GetCollection<Model.Package>("Package");
            return collection
               .Find(
                        Query.And(
                            Query.EQ("TitleIds", BsonValue.Create(titleIds)), //exact order match
                        Query.Or(
                            Query.In("DestinationCode", BsonArray.Create(destinationCodes)), //destination code is present in specified airing
                            Query.EQ("DestinationCode", string.Empty), //destination code is blank
                            Query.NotExists("DestinationCode") //destination code not present
                    ))).ToList();
        }

        public Model.Package GetBy(List<int> titleIds, string destinationCode, string type)
        {
            var collection = _database
                .GetCollection<Model.Package>("Package");
            var qc = new List<IMongoQuery>();
            qc.Add(Query.EQ("TitleIds", BsonValue.Create(titleIds)));
            qc.Add(Query.EQ("Type", type));
            if (!string.IsNullOrEmpty(destinationCode))
                qc.Add(Query.EQ("DestinationCode", destinationCode));
            return collection.Find(Query.And(qc)).FirstOrDefault();
        }
    }
}