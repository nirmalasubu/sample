using OnDemandTools.DAL.Modules.AiringId;
using OnDemandTools.DAL.Modules.AiringId.Model;
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

        public CurrentAiringId Distribute(string prefix)
        {
            //TODO 
            return null;

            //lock (Door)
            //{
            //    var lastAiringId = _query.Get(prefix);

            //    var nextAiringId = lastAiringId.SequenceNumber > 0
            //        ? _creator.Create(lastAiringId.Prefix, lastAiringId.SequenceNumber)
            //        : _creator.Create(lastAiringId.Prefix);

            //    nextAiringId.Id = Convert.ToString(lastAiringId.Id);

            //    nextAiringId.BillingNumber = lastAiringId.BillingNumber;
            //    nextAiringId.BillingNumber.Increment();

            //    _command.Save(nextAiringId);

            //    return nextAiringId;    
            //}
        }
    }
}