using System;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.QueueMessages.Model;
using MongoDB.Driver.Builders;

namespace OnDemandTools.DAL.Modules.QueueMessages.Commands
{
    public class QueueMessageRecorder : IQueueMessageRecorder
    {
        private readonly MongoCollection<HistoricalMessage> _history;

        public QueueMessageRecorder(IODTDatastore connection)
        {
            var database = connection.GetDatabase();

            _history = database.GetCollection<HistoricalMessage>("MessageHistory");
        }

        public void Record(HistoricalMessage record)
        {
            _history.Save(record);
        }

        public void Remove(string mediaId)
        {
            _history.Remove(Query.EQ("MediaId", mediaId));
        }
    }
}