using AutoMapper;
using MongoDB.Bson;
using OnDemandTools.Business.Modules.UserPermissions.Model;
using System.Collections.Generic;
using System.Security.Claims;
using BLModel = OnDemandTools.Common.Configuration;

namespace OnDemandTools.Common.EntityMapping
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserPermission, BLModel.UserIdentity>();

            CreateMap<BLModel.UserIdentity, UserPermission>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
        }
    }

    public class ClaimsResolver : IValueResolver<UserPermission, BLModel.UserIdentity, IEnumerable<Claim>>
    {
        public IEnumerable<Claim> Resolve(UserPermission src, BLModel.UserIdentity des, IEnumerable<Claim> d, ResolutionContext context)
        {
            foreach (string c in src.Api.Claims)
            {
                des.AddClaim(new Claim(c, c));
            }

            return des.Claims;            
        }
    }
}