using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using DLModel = OnDemandTools.DAL.Modules.Pathing.Model;
using System.Collections.Generic;
using System;

namespace OnDemandTools.DAL.Modules.Pathing.Command
{
    public class PathTranslationCommand : IPathTranslationCommand
    {
        private readonly MongoDatabase _database;
        private readonly MongoCollection<DLModel.PathTranslation> _pathTranslations;

        public PathTranslationCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
            _pathTranslations = _database.GetCollection<DLModel.PathTranslation>("PathTranslation");
        }

        /// <summary>
        /// Delete path translation that matches the given object id. 
        /// Return error if it doesn't exist.
        /// </summary>
        /// <param name="id">Path translation object id</param>  
        public void Delete(string id)
        {
            IMongoQuery query = Query.EQ("_id", new ObjectId(id));
            WriteConcernResult remove = _pathTranslations.Remove(query);
        }


        /// <summary>
        /// Save the given path translation model. If it already exist,
        /// update it; else, create a new one.
        /// </summary>
        /// <param name="model">Path translation model</param>
        /// <returns>Newly added or updated path translation model</returns>
        public DLModel.PathTranslation Save(DLModel.PathTranslation pathTranslation)
        {
            _pathTranslations.Save(pathTranslation);
            return pathTranslation;
        }
    }
}