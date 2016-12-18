using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using OnDemandTools.DAL.Modules.AiringId.Model;

namespace OnDemandTools.DAL.Modules.AiringId
{
    public interface IAiringIdDeleteCommand
    {
        void Delete(ObjectId id);
    }

    public interface IAiringIdSaveCommand
    {
        CurrentAiringId Save(CurrentAiringId currentAiringId);
    }

    public interface IGetAiringIdsQuery
    {
        IQueryable<CurrentAiringId> Get();

        CurrentAiringId Get(string prefix);
    }

    public interface IGetLastAiringIdQuery
    {
        CurrentAiringId Get(string prefix);       
    }
}
