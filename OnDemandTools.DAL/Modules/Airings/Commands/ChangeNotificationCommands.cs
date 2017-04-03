﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Airings.Model;
using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.Airings.Commands
{
    public class ChangeNotificationCommands : IChangeNotificaitonCommands
    {
        private readonly MongoCollection<Airing> _collection;

        public ChangeNotificationCommands(IODTDatastore connection)
        {
            MongoDatabase database = connection.GetDatabase();

            _collection = database.GetCollection<Airing>("currentassets");
        }

        public void Save(string airingId, IEnumerable<ChangeNotification> changeNotifications)
        {
            var query = Query.EQ("AssetId", airingId);

            var upd = Update.PushAll("ChangeNotifications", new BsonArray(changeNotifications));

            _collection.Update(query, upd);
        }
    }

    public interface IChangeNotificaitonCommands
    {
        void Save(string airingId, IEnumerable<ChangeNotification> changeNotifications);
    }
}
