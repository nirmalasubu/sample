using BLModel = OnDemandTools.Business.Modules.Queue.Model;
using DLModel = OnDemandTools.DAL.Modules.QueueMessages.Model;
using AutoMapper;

namespace OnDemandTools.Common.EntityMapping.EntityMapping.Rules
{
    public class HistoricalMessageProfile : Profile
    {
        public HistoricalMessageProfile()
        {
            CreateMap<DLModel.HistoricalMessage, BLModel.HistoricalMessage>();
        }
    }
}
