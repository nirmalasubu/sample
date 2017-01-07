using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;
using System;
using System.Linq;

namespace OnDemandTools.DAL.Modules.AiringId.Queries
{
    public class GetLastAiringIdQuery : IGetLastAiringIdQuery
    {
        private readonly MongoDatabase _database;

        public GetLastAiringIdQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public CurrentAiringId Get(string prefix)
        {
            var query = _database.GetCollection<CurrentAiringId>("CurrentAiringId").AsQueryable<CurrentAiringId>();

            try
            {
                return query.First(a => a.Prefix == prefix);
            }
            catch (Exception ex)
            {
                var message = string.Format("An airing id prefix does not exist for '{0}'. You must create and airing id prefix before sending this request.", prefix);

                throw new Exception(message, ex); ;
            }
        }


        //TODO - remove

        public CurrentAiringId Gett(string prefix)
        {

            try
            {
                var currentAiringIds = _database.GetCollection<CurrentAiringId>("CurrentAiringId_A");
                var query = Query.And(Query.EQ("Prefix", prefix), Query.EQ("Locked", "false"));


                CurrentAiringId lastCurrentAiring;
                while (true)
                {
                    var findAndModifyResult = currentAiringIds.FindAndModify(
                    new FindAndModifyArgs()
                    {
                        Query = query,
                        Update = Update.Set("Locked", "true"),
                        VersionReturned = FindAndModifyDocumentVersion.Modified

                    });

                    lastCurrentAiring = findAndModifyResult.GetModifiedDocumentAs<CurrentAiringId>();

                    if (lastCurrentAiring != null)
                        break;
                }

                return lastCurrentAiring;
            }
            catch (Exception ex)
            {
                var message = string.Format("An airing id prefix does not exist for '{0}'. You must create and airing id prefix before sending this request.", prefix);

                throw new Exception(message, ex); ;
            }
        }

        //TODO - remove
        public void Sett(CurrentAiringId ca)
        {
            try
            {
                var currentAiringIds = _database.GetCollection<CurrentAiringId>("CurrentAiringId_A");
                var query = Query.And(Query.EQ("Prefix", ca.Prefix), Query.EQ("Locked", "true"));

                ca.Id = new MongoDB.Bson.ObjectId();
                _database.GetCollection<CurrentAiringId>("CurrentAiringId_B")
                    .Insert(ca);


                CurrentAiringId lastCurrentAiring;
                var findAndModifyResult = currentAiringIds.FindAndModify(
                    new FindAndModifyArgs()
                    {
                        Query = query,
                        Update = Update.Set("Locked", "false")
                        .Set("AiringId", ca.AiringId)
                        .Set("Prefix", ca.Prefix)
                        .Set("SequenceNumber", ca.SequenceNumber)
                        .Set("BillingNumber", ca.BillingNumber.ToBsonDocument())
                        .Set("CreatedBy", ca.CreatedBy)
                        .Set("CreatedDateTime", ca.CreatedDateTime)
                        .Set("ModifiedBy", ca.ModifiedBy)
                        .Set("ModifiedDateTime", ca.ModifiedDateTime),
                        VersionReturned = FindAndModifyDocumentVersion.Modified

                    });

                lastCurrentAiring = findAndModifyResult.GetModifiedDocumentAs<CurrentAiringId>();

                if (lastCurrentAiring == null)
                    throw new Exception("race condition");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}