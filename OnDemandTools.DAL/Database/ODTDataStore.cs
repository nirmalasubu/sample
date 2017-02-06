using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using OnDemandTools.Common.Configuration;
using System;
using System.Linq;

namespace OnDemandTools.DAL.Database
{
    public class ODTDatastore : IODTDatastore
    {
        private readonly MongoDatabase _database;

        public ODTDatastore(AppSettings appSettings)
        {
            _database = GetDatabase(appSettings.MongoDB.ConnectionString, appSettings.MongoDB.ConnectionOptionsDefault);
        }

        private MongoDatabase GetDatabase(string connectionString, string options)
        {
            var url = new MongoUrl(connectionString + options);

            var client = new MongoClient(url);
            var server = client.GetServer();
            return server.GetDatabase(url.DatabaseName);
        }


        /// <summary>
        /// Get's the database with Secondary Read Preference
        /// </summary>
        /// <returns></returns>
        public MongoDatabase GetDatabase()
        {
            return _database;
        }
    }


    public class HangfireDatastore : IHangfireDatastore
    {
        private readonly MongoDatabase _database;

        public HangfireDatastore(AppSettings appSettings)
        {
            _database = GetDatabase(appSettings.MongoDB.HangfireConnectionString);
        }

        private MongoDatabase GetDatabase(string connectionString)
        {
            var url = new MongoUrl(connectionString);

            var client = new MongoClient(url);
            var server = client.GetServer();
            return server.GetDatabase(url.DatabaseName);
        }


        /// <summary>
        /// Get's the database with Secondary Read Preference
        /// </summary>
        /// <returns></returns>
        public MongoDatabase GetDatabase()
        {
            return _database;
        }
    }


    public class ODTPrimaryDatastore : IODTDatastore, IODTPrimaryDatastore
    {
        private readonly MongoDatabase _primaryDatabase;
        public ODTPrimaryDatastore(AppSettings appSettings)
        {
            _primaryDatabase = GetDatabase(appSettings.MongoDB.ConnectionString, appSettings.MongoDB.ConnectionOptionsDefault);
        }

        private MongoDatabase GetDatabase(string connectionString, string options)
        {
            var url = new MongoUrl(connectionString + options);

            var client = new MongoClient(url);
            var server = client.GetServer();

            return server.GetDatabase(url.DatabaseName);
        }

        public MongoDatabase GetDatabase()
        {
            return _primaryDatabase;
        }
    }

    public interface IODTDatastore
    {
        MongoDatabase GetDatabase();
    }

    public interface IODTPrimaryDatastore
    {
        MongoDatabase GetDatabase();
    }

    public interface IHangfireDatastore
    {
        MongoDatabase GetDatabase();
    }

}