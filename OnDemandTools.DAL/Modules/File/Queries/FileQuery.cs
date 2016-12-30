using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Modules.Airings.Model;
using OnDemandTools.DAL.Modules.Airings.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using OnDemandTools.DAL.Database;
using FileModel = OnDemandTools.DAL.Modules.File.Model;

namespace OnDemandTools.DAL.Modules.File.Queries
{

    public class FileQuery : IFileQuery
    {
        private readonly MongoDatabase _database;

        public FileQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public List<FileModel.File> Get(int titleId)
        {
            var collection = _database.GetCollection<FileModel.File>("File");

            var query = Query.EQ("TitleId", titleId);

            return collection.Find(query).ToList();
        }

        public List<FileModel.File> Get(string airingId)
        {
            var fileCollection = _database.GetCollection<FileModel.File>("File");
            var assetCollection = _database.GetCollection<Airing>("currentassets");

            // Find Airing with the corresponding airingId
            var airingQuery = Query.EQ("AssetId", airingId);
            Airing airing = assetCollection.Find(airingQuery).FirstOrDefault();
            if (airing == null)
                throw new AiringNotFoundException(string.Format("Airing with id {0} does not exist in collection.", airingId));
            
            // Verify if MediaId is empty
            String mediaId = string.Empty;
            if(!String.IsNullOrWhiteSpace(airing.MediaId))
            {
                mediaId = airing.MediaId;
            }

            var query = Query.Or(Query.EQ("MediaId", mediaId), Query.EQ("AiringId", airingId));
            return fileCollection.Find(query).Select(c=> {c.AiringId = airingId; return c;}).ToList();
        }

        public IList<FileModel.File> GetBy(List<int> titleIds)
        {
            var collection = _database.GetCollection<FileModel.File>("File");

            var query = Query.In("TitleId", new BsonArray(titleIds));

            return collection.Find(query).ToList();
        }

        public IList<FileModel.File> GetBy(List<string> contentIds, List<int> titleIds, string airingId, string mediaId)
        {
            var collection = _database.GetCollection<FileModel.File>("File");

            var files = collection.Find(Query.Or(
                    Query.In("TitleId", new BsonArray(titleIds)),                   
                    Query.EQ("AiringId", airingId),
                    Query.EQ("MediaId", mediaId))).ToList();

            return files;
        }
    }
}