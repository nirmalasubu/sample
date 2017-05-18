using AutoMapper;
using OnDemandTools.Web.Models.Status;
using BLModel = OnDemandTools.Business.Modules.Status.Model;


namespace OnDemandTools.Web.Mappings
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<StatusModel, BLModel.Status>();
            CreateMap<BLModel.Status, StatusModel>();
        }
    }
}
