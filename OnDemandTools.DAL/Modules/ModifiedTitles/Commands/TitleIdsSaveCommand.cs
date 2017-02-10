using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;
using System.Collections.Generic;
using System.Linq;

namespace OnDemandTools.DAL.Modules.ModifiedTitles.Commands
{
    public class TitleIdsSaveCommand : ITitleIdsCommand
    {
        private readonly MongoCollection<ModifiedTitle> _modifiedTitle;

        public TitleIdsSaveCommand(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _modifiedTitle = database.GetCollection<ModifiedTitle>("ModifiedTitle");
        }

        public void Save(IList<int> titleIds)
        {
            if (titleIds.Count == 0)
                return;

            var modifiedTitles = titleIds.Select(t => new ModifiedTitle(t));

            _modifiedTitle.InsertBatch(modifiedTitles);
        }

        public void Delete(IList<int> titleIds)
        {
            _modifiedTitle.Remove(Query.In("TitleId", new BsonArray(titleIds)));
        }
    }

    public class ModifiedTitle
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TitleId { get; set; }

        public ModifiedTitle(int titleId)
        {
            TitleId = titleId;
        }
    }
}