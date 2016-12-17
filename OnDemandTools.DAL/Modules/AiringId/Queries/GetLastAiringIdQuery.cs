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

        public GetLastAiringIdQuery(ODTPrimaryDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public CurrentAiringId Get(string prefix)
        {
            var currentAiringIds = _database.GetCollection<CurrentAiringId>("CurrentAiringId");
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

                Console.WriteLine("waiting ");

            }


            //FindAndModifyArgs args = new FindAndModifyArgs();
            //args.Query = filter;
            //args.Update = Update.Set("Locked", true);
            //args.

            //var query = .FindAndModify()
            //var query = _database.GetCollection<CurrentAiringId>("CurrentAiringId").AsQueryable<CurrentAiringId>();

            try
            {
                //return query.First(a => a.Prefix == prefix);
                return lastCurrentAiring;
            }
            catch (Exception ex)
            {
                var message = string.Format("An airing id prefix does not exist for '{0}'. You must create and airing id prefix before sending this request.", prefix);

                throw new Exception(message, ex);;
            }
        }


        public void Set(CurrentAiringId ca)
        {
           

            //FindAndModifyArgs args = new FindAndModifyArgs();
            //args.Query = filter;
            //args.Update = Update.Set("Locked", true);
            //args.

            //var query = .FindAndModify()
            //var query = _database.GetCollection<CurrentAiringId>("CurrentAiringId").AsQueryable<CurrentAiringId>();

            try
            {
                var currentAiringIds = _database.GetCollection<CurrentAiringId>("CurrentAiringId");
                var query = Query.And(Query.EQ("Prefix", ca.Prefix), Query.EQ("Locked", "true"));

                CurrentAiringId lastCurrentAiring;
                var findAndModifyResult = currentAiringIds.FindAndModify(
                    new FindAndModifyArgs()
                    {
                        Query = query,
                        Update = Update.Set("Locked", "false").Set("SequenceNumber", (++ca.SequenceNumber).ToString()),
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