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
        private Object lockObject = new Object();

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
            bool isLocked = false;
            BLModel.CurrentAiringId nextAiringId = null;

            lock (lockObject)
            {
                if (_query.Get(prefix) == null)
                {
                    var message = string.Format("An airing id prefix does not exist for '{0}'. You must create and airing id prefix before sending this request.", prefix);
                    throw new Exception(message);
                }

                try
                {
                    nextAiringId = _command.Lock(prefix).ToBusinessModel<CurrentAiringId, BLModel.CurrentAiringId>(); ;
                    if (nextAiringId == null)
                    {
                        var message = string.Format("Null value returned from  lock  '{0}'", prefix);
                        throw new Exception(message);
                    }
                    isLocked = true;
                    var result = nextAiringId.SequenceNumber > 0
                       ? _creator.Create(nextAiringId.Prefix, nextAiringId.SequenceNumber)
                       : _creator.Create(nextAiringId.Prefix);
                    nextAiringId.AiringId = result.AiringId;
                    nextAiringId.BillingNumber.Increment();

                    nextAiringId.ModifiedDateTime = DateTime.UtcNow;
                    _command.UpdateAndUnlock(nextAiringId.ToDataModel<BLModel.CurrentAiringId, CurrentAiringId>());
                }
                catch (Exception ex)
                {
                    if (isLocked)
                    {
                        _command.UnLock(prefix);
                    }
                    throw ex;
                }
            }
            return nextAiringId;
        }

    }
}