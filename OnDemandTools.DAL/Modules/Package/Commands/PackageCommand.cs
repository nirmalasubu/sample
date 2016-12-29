using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.Models.History;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.Package.Commands
{
    public class PackageCommand : IPackageCommand
    {
        private readonly MongoDatabase _database;

        public PackageCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public Model.Package Save(Model.Package packageDataModel, string userName, bool updateHistorical)
        {
            var collection = _database.GetCollection<Model.Package>(DataStoreConfiguration.CurrentPackagesCollection);
            var deletedCollection = _database.GetCollection<Model.Package>(DataStoreConfiguration.DeletedPackagesCollection);
            var historicalCollection = _database.GetCollection<Model.Package>(DataStoreConfiguration.HistoricalPackagesCollection);

            var qc = new List<IMongoQuery>();
            qc.Add(Query.EQ("TitleIds", BsonValue.Create(packageDataModel.TitleIds)));
            qc.Add(Query.EQ("Type", packageDataModel.Type));
            if (!string.IsNullOrEmpty(packageDataModel.DestinationCode))
                qc.Add(Query.EQ("DestinationCode", packageDataModel.DestinationCode));
            else //we need to explicitly add a query to exclude
                qc.Add(Query.NotExists("DestinationCode"));


            Model.Package matchingPkg = collection
                .Find(Query.And(qc))
                .AsQueryable().FirstOrDefault();

            Model.Package deletedPkg = deletedCollection
                .Find(Query.And(qc))
                .AsQueryable().FirstOrDefault();

            //match for a previously "deleted" package           
            if (deletedPkg != null)
                deletedCollection.Remove(Query.And(qc)); //remove it       

            if (matchingPkg != null) //if there is already a package with same TitleIds, DestinationCode and Type in Package -- then overwrite the package.
            {
                matchingPkg.PackageData = packageDataModel.PackageData;
                collection.Update(Query.EQ("_id", matchingPkg.Id), Update.Replace(matchingPkg));
                if (updateHistorical)
                    historicalCollection.Save(new HistoricalRecord(packageDataModel, "MODIFY", userName));
                return matchingPkg;
            }
            else //otherwise simply store it
            {
                collection.Save(packageDataModel);
                if (updateHistorical)
                    historicalCollection.Save(new HistoricalRecord(packageDataModel, "CREATE", userName));
                return packageDataModel;
            }
        }

        public Model.Package Delete(Model.Package pkg, string userName, bool updateHistorical)
        {
            var currentCollection = _database.GetCollection<Model.Package>(DataStoreConfiguration.CurrentPackagesCollection);
            var deletedCollection = _database.GetCollection<Model.Package>(DataStoreConfiguration.DeletedPackagesCollection);
            var historicalCollection = _database.GetCollection<HistoricalRecord>(DataStoreConfiguration.HistoricalPackagesCollection);

            if (updateHistorical)
                historicalCollection.Save(new HistoricalRecord(pkg, "DELETE", userName));

            var qc = new List<IMongoQuery>();
            qc.Add(Query.EQ("TitleIds", BsonValue.Create(pkg.TitleIds)));
            qc.Add(Query.EQ("Type", pkg.Type));
            if (!string.IsNullOrEmpty(pkg.DestinationCode))
                qc.Add(Query.EQ("DestinationCode", pkg.DestinationCode));
            else //we need to explicitly add a query to exclude
                qc.Add(Query.NotExists("DestinationCode"));

            currentCollection.Remove(Query.And(qc));

            deletedCollection.Update(Query.And(qc),
                Update.Replace(pkg),
                UpdateFlags.Upsert);

            return pkg;
        }
    }
}