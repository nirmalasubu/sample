using AutoMapper;
using OnDemandTools.Web.Models.Destination;
using OnDemandTools.Web.Models.UserPermissions;
using BLModel = OnDemandTools.Business.Modules.UserPermissions.Model;


namespace OnDemandTools.Web.Mappings
{
    public class UserPermissionsProfile : Profile
    {
        public UserPermissionsProfile()
        {
            //View Model to Business Model
            CreateMap<Api, BLModel.Api>();
            CreateMap<UserPermission, BLModel.UserPermission>();
            CreateMap<Portal, BLModel.Portal>();
            CreateMap<Permission, BLModel.Permission>();


            //Business Model to view Model
            CreateMap<BLModel.Api, Api>();
            CreateMap<BLModel.UserPermission, UserPermission>();
            CreateMap<BLModel.Portal, Portal>();
            CreateMap<BLModel.Permission, Permission>();                       
        }
    }
}
