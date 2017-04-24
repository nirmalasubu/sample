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
        private static MongoClient _client;

        public ODTDatastore(AppSettings appSettings)
        {
            _database = GetDatabase(appSettings.MongoDB.ConnectionString, appSettings.MongoDB.ConnectionOptionsDefault);
        }

        public static MongoClient GetClient(MongoUrl url)
        {
            if((_client == null)){
                _client = new MongoClient(url);
                return _client;
            }
            else{
                try
                {
                    string ms;
                   _client.GetServer().IsDatabaseNameValid(url.DatabaseName, out ms);
                   return _client;
                }
                catch (System.Exception ex)
                {                    
                     _client = new MongoClient(url);
                    return _client;
                }               
            }
        }

        private MongoDatabase GetDatabase(string connectionString, string options)
        {
            var url = new MongoUrl(connectionString + options);

            var client = GetClient(url);
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
        private static MongoClient _client;

        public HangfireDatastore(AppSettings appSettings)
        {
            _database = GetDatabase(appSettings.MongoDB.HangfireConnectionString);
        }


        public static MongoClient GetClient(MongoUrl url)
        {
            if((_client == null)){
                _client = new MongoClient(url);
                return _client;
            }
            else{
                try
                {
                    string ms;
                   _client.GetServer().IsDatabaseNameValid(url.DatabaseName, out ms);
                   return _client;
                }
                catch (System.Exception ex)
                {                    
                     _client = new MongoClient(url);
                    return _client;
                }               
            }
        }

        private MongoDatabase GetDatabase(string connectionString)
        {
            var url = new MongoUrl(connectionString);

            var client = GetClient(url);
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
         private static MongoClient _client;


        public static MongoClient GetClient(MongoUrl url)
        {
            if((_client == null)){
                _client = new MongoClient(url);
                return _client;
            }
            else{
                try
                {
                    string ms;
                   _client.GetServer().IsDatabaseNameValid(url.DatabaseName, out ms);
                   return _client;
                }
                catch (System.Exception ex)
                {                    
                     _client = new MongoClient(url);
                    return _client;
                }               
            }
        }

        public ODTPrimaryDatastore(AppSettings appSettings)
        {
            _primaryDatabase = GetDatabase(appSettings.MongoDB.ConnectionString, appSettings.MongoDB.ConnectionOptionsDefault);
        }

        private MongoDatabase GetDatabase(string connectionString, string options)
        {
            var url = new MongoUrl(connectionString + options);

            var client = GetClient(url);
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