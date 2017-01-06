using AutoMapper;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Security.Claims;
using BLModel = OnDemandTools.Common.Configuration;
using DLModel = OnDemandTools.DAL.Modules.User.Model;

namespace OnDemandTools.API.Utilities.EntityMapping.Rules
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {

            CreateMap<DLModel.UserIdentity, BLModel.UserIdentity>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()))
                 .ForMember(d => d.Claims, opt => opt.ResolveUsing<ClaimsResolver>());


            CreateMap<BLModel.UserIdentity, DLModel.UserIdentity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
        }
    }

    public class ClaimsResolver : IValueResolver<DLModel.UserIdentity, BLModel.UserIdentity, IEnumerable<Claim>>
    {
        public IEnumerable<Claim> Resolve(DLModel.UserIdentity src, BLModel.UserIdentity des, IEnumerable<Claim> d, ResolutionContext context)
        {
            foreach (string c in src.Claims)
            {
                des.AddClaim(new Claim(c, c));
            }

            return des.Claims;            
        }
    }
}