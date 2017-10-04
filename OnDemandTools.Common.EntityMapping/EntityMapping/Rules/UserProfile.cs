using AutoMapper;
using MongoDB.Bson;
using OnDemandTools.Business.Modules.UserPermissions.Model;
using System.Collections.Generic;
using System.Security.Claims;
using BLModel = OnDemandTools.Common.Configuration;

namespace OnDemandTools.Common.EntityMapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserPermission, BLModel.UserIdentity>()
                  .ForMember(d => d.ApiKey, opt => opt.MapFrom(s => s.Api.ApiKey))
                .ForMember(d => d.BrandPermitAll, opt => opt.MapFrom(s => s.Api.BrandPermitAll))
                .ForMember(d => d.DestinationPermitAll, opt => opt.MapFrom(s => s.Api.DestinationPermitAll))
                .ForMember(d => d.Destinations, opt => opt.MapFrom(s => s.Api.Destinations))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(d => d.UserType, opt => opt.MapFrom(s => s.UserType))
                .ForMember(d => d.Claims, opt => opt.ResolveUsing<ClaimsResolver>());
        }
    }

    public class ClaimsResolver : IValueResolver<UserPermission, BLModel.UserIdentity, IEnumerable<Claim>>
    {
        public IEnumerable<Claim> Resolve(UserPermission src, BLModel.UserIdentity des, IEnumerable<Claim> d, ResolutionContext context)
        {
            AddApiClaims(src, des);

            AddPortalClaims(src, des);

            return des.Claims;
        }

        private static void AddPortalClaims(UserPermission src, BLModel.UserIdentity des)
        {
            if (src.Portal.IsActive)
            {
                des.AddClaim(new Claim("read", "read"));
            }
        }

        private static void AddApiClaims(UserPermission src, BLModel.UserIdentity des)
        {
            if (src.UserType == UserType.Api && src.Api.IsActive  //API should be active to get the claims
                || (src.UserType == UserType.Portal && src.Portal.IsActive && src.Api.IsActive) // For portal user both System and API should be active
               )
            {
                foreach (string c in src.Api.Claims)
                {
                    des.AddClaim(new Claim(c, c));
                }
            }
        }
    }
}