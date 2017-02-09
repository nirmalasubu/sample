using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.QueueMessages.Model;

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
    }
}