using AutoMapper;
using OnDemandTools.API.v1.Models.Package;
using BLModel = OnDemandTools.Business.Modules.Package.Model;

namespace OnDemandTools.API.Helpers.MappingRules.Package
{
    public class PackageViewModelProfile:Profile
    {
        public PackageViewModelProfile()
        {
            CreateMap<BLModel.Package, PackageViewModel>();

            CreateMap<PackageViewModel, BLModel.Package>();
        }
    }
}
