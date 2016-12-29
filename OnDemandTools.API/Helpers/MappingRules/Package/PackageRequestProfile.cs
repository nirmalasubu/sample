using AutoMapper;
using OnDemandTools.API.v1.Models.Package;
using BLModel = OnDemandTools.Business.Modules.Package.Model;

namespace OnDemandTools.API.Helpers.MappingRules.Package
{
    public class PackageRequestProfile: Profile
    {
        public PackageRequestProfile()
        {
            CreateMap<PackageRequest, BLModel.Package>()
                .ForMember(x => x.DestinationCode, map => map
                .MapFrom(p => string.IsNullOrEmpty(p.DestinationCode) ? null : p.DestinationCode));

            CreateMap<BLModel.Package, PackageRequest>();           
        }
    }
}

