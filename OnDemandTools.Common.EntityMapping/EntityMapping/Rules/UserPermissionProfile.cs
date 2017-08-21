using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;
using DLModel = OnDemandTools.DAL.Modules.UserPermissions.Model;

namespace OnDemandTools.Common.EntityMapping
{
    public class UserPermissionProfile : Profile
    {
        public UserPermissionProfile()
        {            
            //Business to Data layer
            CreateMap<BLModel.UserPermission, DLModel.UserPermission>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
            CreateMap<BLModel.Api, DLModel.Api>();
            CreateMap<BLModel.Portal, DLModel.Portal>();
            CreateMap<BLModel.Permission, DLModel.Permission>();
          
            //Data layer to business
            CreateMap<DLModel.UserPermission, BLModel.UserPermission>()
             .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));
            CreateMap<DLModel.Api, BLModel.Api>();
            CreateMap<DLModel.Portal, BLModel.Portal>();
            CreateMap<DLModel.Permission, BLModel.Permission>();
        }
    }
}
