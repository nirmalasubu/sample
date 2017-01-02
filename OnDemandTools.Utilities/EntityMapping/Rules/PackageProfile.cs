using AutoMapper;
using BLModel = OnDemandTools.Business.Modules.Package.Model;
using DLModel = OnDemandTools.DAL.Modules.Package.Model;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using BLAiringLongModel = OnDemandTools.Business.Modules.Airing.Model.Alternate;

namespace OnDemandTools.Utilities.EntityMapping.Rules
{

    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<BLModel.Package, DLModel.Package>()
               .ForMember(x => x.PackageData, map => map.ResolveUsing<PackageDataResolverFromModel>())          
               .ForMember(x => x.DestinationCode, map => map.MapFrom(p => string.IsNullOrEmpty(p.DestinationCode) ? null : p.DestinationCode));


            CreateMap<DLModel.Package, BLModel.Package>()
              .ForMember(x => x.PackageData, map => map.ResolveUsing<PackageDataResolverFromData>())
              .ForMember(x => x.Data, map => map.ResolveUsing<PackageDataStringResolverFromData>());

            CreateMap<DLModel.Package, BLAiringLongModel.Package.Package>()
             .ForMember(x => x.PackageData, map => map.ResolveUsing<PackageDataResolverFromLongData>())
             .ForMember(x => x.Data, map => map.ResolveUsing<PackageDataStringResolverFromLongData>());

        }
    }


   class PackageDataStringResolverFromData : IValueResolver<DLModel.Package, BLModel.Package, string>
    {
        public string Resolve(DLModel.Package src, BLModel.Package des, string d, ResolutionContext context)
        {
            return (src.PackageData != null && !string.IsNullOrEmpty(src.PackageData.ToString())) ?
                src.PackageData.ToString()
                : null;
        }
    }

    class PackageDataStringResolverFromLongData : IValueResolver<DLModel.Package, BLAiringLongModel.Package.Package, string>
    {
        public string Resolve(DLModel.Package src, BLAiringLongModel.Package.Package des, string d, ResolutionContext context)
        {
            return (src.PackageData != null && !string.IsNullOrEmpty(src.PackageData.ToString())) ?
                src.PackageData.ToString()
                : null;
        }
    }

    class PackageDataResolverFromData : IValueResolver<DLModel.Package, BLModel.Package, object>
    {
        public object Resolve(DLModel.Package src, BLModel.Package des, object d, ResolutionContext context)
        {
            return (src.PackageData != null && !string.IsNullOrEmpty(src.PackageData.ToString())) ?
                BsonSerializer.Deserialize<object>(src.PackageData)
                : null;
        }
    }

    class PackageDataResolverFromLongData : IValueResolver<DLModel.Package, BLAiringLongModel.Package.Package, object>
    {
        public object Resolve(DLModel.Package src, BLAiringLongModel.Package.Package des, object d, ResolutionContext context)
        {
            return (src.PackageData != null && !string.IsNullOrEmpty(src.PackageData.ToString())) ?
                BsonSerializer.Deserialize<object>(src.PackageData)
                : null;
        }
    }


    class PackageDataResolverFromModel : IValueResolver<BLModel.Package, DLModel.Package, BsonDocument>
    {
        public BsonDocument Resolve(BLModel.Package src, DLModel.Package des, BsonDocument d, ResolutionContext context)
        {
            return (src.PackageData != null && !string.IsNullOrEmpty(src.PackageData.ToString())) ?
               BsonSerializer.Deserialize<BsonDocument>(src.PackageData.ToString())
               : null;
        }
    }

}
