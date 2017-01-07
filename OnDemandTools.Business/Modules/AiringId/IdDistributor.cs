using OnDemandTools.Common.Model;
using OnDemandTools.DAL.Modules.AiringId;
using OnDemandTools.DAL.Modules.AiringId.Model;
using BLModel = OnDemandTools.Business.Modules.AiringId.Model;
using System;

namespace OnDemandTools.Business.Modules.AiringId
{
    public class IdDistributor : IIdDistributor
    {
        private readonly IAiringIdCreator _creator;
        private readonly IGetLastAiringIdQuery _query;
        private readonly IAiringIdSaveCommand _command;
        private static readonly object Door = new object();

        //TODO - remove (also remove from constrcutor
        Common.Configuration.AppSettings appSettings;

        public IdDistributor(
            IAiringIdCreator creator,
            IGetLastAiringIdQuery query,
            IAiringIdSaveCommand command,
            Common.Configuration.AppSettings appSettings)
        {
            _creator = creator;
            _query = query;
            _command = command;

            //TODO - remove
            this.appSettings = appSettings;
        }

        public BLModel.CurrentAiringId Distribute(string prefix)
        {

            lock (Door)
            {
                var lastAiringId = _query.Get(prefix).ToBusinessModel<CurrentAiringId, BLModel.CurrentAiringId>();

                var nextAiringId = lastAiringId.SequenceNumber > 0
                    ? _creator.Create(lastAiringId.Prefix, lastAiringId.SequenceNumber)
                    : _creator.Create(lastAiringId.Prefix);

                nextAiringId.Id = Convert.ToString(lastAiringId.Id);
                nextAiringId.BillingNumber = lastAiringId.BillingNumber;
                nextAiringId.BillingNumber.Increment();

                return
                    (_command.Save(nextAiringId.ToDataModel<BLModel.CurrentAiringId, CurrentAiringId>())
                    .ToBusinessModel<CurrentAiringId, BLModel.CurrentAiringId>());
           
            }
        }

        //TODO - remove
        public BLModel.CurrentAiringId Unleash(string prefix)
        {
            UnleashCreator unCreator = new UnleashCreator();
            OnDemandTools.DAL.Modules.AiringId.Queries.GetLastAiringIdQuery gq = new OnDemandTools.DAL.Modules.AiringId.Queries.GetLastAiringIdQuery(new OnDemandTools.DAL.Database.ODTPrimaryDatastore(this.appSettings));
            BLModel.CurrentAiringId lastAiringId = gq.Gett(prefix).ToBusinessModel<CurrentAiringId, BLModel.CurrentAiringId>();
            var nextAiringId = lastAiringId.SequenceNumber > 0
                ? unCreator.Create(lastAiringId.Prefix, lastAiringId.SequenceNumber)
                : unCreator.Create(lastAiringId.Prefix);

            nextAiringId.Id = lastAiringId.Id;
            nextAiringId.BillingNumber = lastAiringId.BillingNumber;
            nextAiringId.BillingNumber.Increment();
            nextAiringId.ModifiedDateTime = DateTime.UtcNow;
            nextAiringId.ModifiedBy = "jjohn";
            nextAiringId.CreatedBy = "jjohn";
            gq.Sett(nextAiringId.ToDataModel<BLModel.CurrentAiringId, CurrentAiringId>());

            return nextAiringId;
        }
    }
}