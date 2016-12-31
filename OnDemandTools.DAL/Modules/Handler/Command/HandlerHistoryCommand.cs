using System;

using MongoDB.Driver;

using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.Handler.Model;
using MongoDB.Bson;

namespace OnDemandTools.DAL.Modules.Handler.Command
{
    /// <summary>
    /// Concrete CRUD operations for HandlerHistory
    /// </summary>

    public class HandlerHistoryCommand : IHandlerHistoryCommand
    {

        private readonly MongoDatabase _database;
        
        ///<summary>
        /// Constructor
        ///</summary>
        public HandlerHistoryCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        /// <summary>
        /// Saves the specified encoding raw payload.
        /// </summary>
        /// <param name="encodingRawPayload">The encoding raw payload.</param>
        public void Save(HandlerHistory encodingPayload, string userName, string handlerHistoryJSON)
        {
            encodingPayload.RawJSONPayload = BsonDocument.Parse(handlerHistoryJSON);
            encodingPayload.CreatedDateTime = DateTime.UtcNow;
            encodingPayload.CreatedBy = userName;

            var handlerHistoryCollection = _database.GetCollection<HandlerHistory>(DataStoreConfiguration.HandlerHistoryCollection);
            handlerHistoryCollection.Save(encodingPayload);
        }

    }
}
