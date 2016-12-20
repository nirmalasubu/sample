using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace OnDemandTools.DAL.Helpers
{
    public class JsonToQueryConverter
    {
        public QueryDocument Convert(string jsonQuery)
        {
            if (jsonQuery == null)
                return new QueryDocument();

            try
            {
                var query = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonQuery);

                var queryDoc = new QueryDocument(query);

                return queryDoc;
            }
            catch (Exception ex)
            { 
                throw new QueryValidationException("Incorrect query syntax.", ex);
            }
        }
    }

    public class QueryValidationException : Exception
    {
        public QueryValidationException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}