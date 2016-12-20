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

        public IdDistributor(
            IAiringIdCreator creator,
            IGetLastAiringIdQuery query,
            IAiringIdSaveCommand command)
        {
            _creator = creator;
            _query = query;
            _command = command;
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
    }
}