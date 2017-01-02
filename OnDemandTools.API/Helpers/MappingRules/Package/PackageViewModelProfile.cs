using AutoMapper;
using OnDemandTools.API.v1.Models.Package;
using BLModel = OnDemandTools.Business.Modules.Package.Model;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;
using VMAiringModel = OnDemandTools.API.v1.Models.Airing;

namespace OnDemandTools.API.Helpers.MappingRules.Package
{
    public class PackageViewModelProfile:Profile
    {
        public PackageViewModelProfile()
        {
            CreateMap<BLModel.Package, PackageViewModel>();

            CreateMap<PackageViewModel, BLModel.Package>();

            CreateMap<BLAiringLongModel.Package.Package, VMAiringModel.Package.Package>();
        }
    }
}
