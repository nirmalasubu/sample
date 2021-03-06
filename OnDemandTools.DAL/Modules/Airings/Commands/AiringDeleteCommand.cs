﻿using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Reporting.Command;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class AiringDeleteCommand : IAiringDeleteCommand
    {
        private readonly MongoDatabase _database;
        private readonly IDfStatusMover _dfStatusMover;

        public AiringDeleteCommand(IODTDatastore connection,
            IDfStatusMover dfStatusMover)
        {
            _database = connection.GetDatabase();
            _dfStatusMover = dfStatusMover;
        }

        public Airing Delete(Airing airing)
        {
            var currentCollection = _database.GetCollection<CurrentAiringId>(DataStoreConfiguration.CurrentAssetsCollection);
            var deletedCollection = _database.GetCollection<CurrentAiringId>(DataStoreConfiguration.DeletedAssetsCollection);
            var expiredCollection =
                _database.GetCollection<CurrentAiringId>(DataStoreConfiguration.ExpiredAssetsCollection);

            currentCollection.Remove(Query.EQ("AssetId", airing.AssetId));
            expiredCollection.Remove(Query.EQ("AssetId", airing.AssetId));


            deletedCollection.Update(Query.EQ("AssetId", airing.AssetId),
                Update.Replace(airing),
                UpdateFlags.Upsert);

            _dfStatusMover.MoveToExpireCollection(airing.AssetId);

            return airing;
        }
    }
}