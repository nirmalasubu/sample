using System;
using System.Security.Principal;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;
using OnDemandTools.DAL.Modules.AiringId.Model;
using MongoDB.Driver.Builders;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.DAL.Modules.AiringId.Commands
{
    public class AiringIdSaveCommand : IAiringIdSaveCommand
    {
        private readonly MongoDatabase _database;
        private readonly IApplicationContext _appContext;
        private readonly AppSettings _appSettings;

        // Database connection that creates a new client for every request
        private readonly MongoDatabase _databaseWithNewClient;
        public AiringIdSaveCommand(IODTDatastore connection, IApplicationContext appContext, AppSettings appSettings)
        {
            _databaseWithNewClient = connection.GetDatabaseSingleClient();
            _database = connection.GetDatabase();
            _appContext = appContext;
            _appSettings = appSettings;

        }

        #region PUBLIC METHODS

        public CurrentAiringId Save(CurrentAiringId currentAiringId)
        {
            var collection = _database.GetCollection<CurrentAiringId>("CurrentAiringId");
            currentAiringId.ModifiedDateTime = DateTime.UtcNow;
            collection.Save(currentAiringId);
            return currentAiringId;
        }

        public CurrentAiringId Lock(string prefix)
        {
            var currentAiringIds =  _databaseWithNewClient.GetCollection<CurrentAiringId>("CurrentAiringId");

            var query = Query.And(Query.EQ("Prefix", prefix), Query.NE("State", "locked"));

            CurrentAiringId currentAiringId = null;

            try
            {
                var entrytime = DateTime.Now;

                while (true)
                {
                    var seconds = DateTime.Now.Subtract(entrytime).Seconds;

                    if (seconds > int.Parse(_appSettings.AiringIdLockExpiredSeconds))
                    {
                        UnLock(prefix);
                    }

                    var findAndModifyResult = currentAiringIds.FindAndModify(new FindAndModifyArgs()
                    {
                        Query = query,
                        Update = Update.Set("State", "locked")
                                       .Set("Locked", true)
                                       .Inc("SequenceNumber", 1),
                        VersionReturned = FindAndModifyDocumentVersion.Modified
                    });

                    currentAiringId = findAndModifyResult.GetModifiedDocumentAs<CurrentAiringId>();

                    if (currentAiringId != null)
                    {
                        if (currentAiringId.SequenceNumber > 99999)
                        {
                            ResetSequenceNumber(currentAiringId);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("Error occured while aquiring lock for {0} :{1} ", prefix, ex.StackTrace);
                throw new Exception(message, ex);
            }

            return currentAiringId;
        }

        public void UnLock(string prefix)
        {
            var currentAiringIds =  _databaseWithNewClient.GetCollection<CurrentAiringId>("CurrentAiringId");

            var query = Query.EQ("Prefix", prefix);

            currentAiringIds.Update(query,
                                            Update.Set("State", "unlocked")
                                                  .Set("Locked", false),
                                            WriteConcern.W2);
        }

        public void UpdateAndUnlock(CurrentAiringId currentAiringId)
        {
            var currentAiringIds =  _databaseWithNewClient.GetCollection<CurrentAiringId>("CurrentAiringId");

            var query = Query.And(Query.EQ("Prefix", currentAiringId.Prefix), Query.Or(Query.EQ("State", "locked"), Query.EQ("Locked", true)));

            try
            {
                currentAiringId.ModifiedBy = _appContext.GetUser()!=null?_appContext.GetUser().UserName: _appContext.GetUserName();

                var findAndModifyResult = currentAiringIds.FindAndModify(
                    new FindAndModifyArgs()
                    {
                        Query = query,
                        Update = Update.Set("State", "unlocked")
                                       .Set("Locked", false)
                                       .Set("BillingNumber.Current", currentAiringId.BillingNumber.Current)
                                       .Set("SequenceNumber", currentAiringId.SequenceNumber)
                                       .Set("AiringId", currentAiringId.AiringId)
                                       .Set("ModifiedBy", currentAiringId.ModifiedBy)
                                       .Set("ModifiedDateTime", currentAiringId.ModifiedDateTime)
                                       .Push("GenratedAiringIds", currentAiringId.AiringId),
                        VersionReturned = FindAndModifyDocumentVersion.Modified
                    });

                if (findAndModifyResult.GetModifiedDocumentAs<CurrentAiringId>() == null)
                {
                    throw new Exception("unable to unlock the airingId");
                }
            }

            catch (Exception ex)
            {
                var message = string.Format("Error occured while updating and ulock the document for {0} :{1}:{2} ", currentAiringId.AiringId, currentAiringId.SequenceNumber, ex.Message);
                throw new Exception(message, ex);
            }


        }

        #endregion

        #region PRIVATE METHODS

        private void ResetSequenceNumber(CurrentAiringId currentAiringId)
        {
            currentAiringId.SequenceNumber = 1;
            Save(currentAiringId);
        }
        #endregion


    }

}