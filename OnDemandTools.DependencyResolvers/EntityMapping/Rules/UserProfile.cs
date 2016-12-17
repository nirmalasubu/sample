using AutoMapper;
using MongoDB.Bson;
using BLModel = OnDemandTools.Business.Modules.User.Model;
using DLModel = OnDemandTools.DAL.Modules.User.Model;

namespace OnDemandTools.Utilities.EntityMapping.Rules
{
    public class UserProfile: Profile
    {
        protected override void Configure()
        {

            CreateMap<DLModel.UserIdentity, BLModel.UserIdentity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));

            CreateMap<BLModel.UserIdentity, DLModel.UserIdentity>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Id) ? new ObjectId() : new ObjectId(s.Id)));
        }
    }
}