using MongoDB.Bson;
using System;
using System.Linq;
namespace OnDemandTools.DAL.Modules.User.Queries
{
    public interface IGetUsersQuery
    {
        IQueryable<Model.UserIdentity> GetUsers();

        Model.UserIdentity GetById(String id);

        Model.UserIdentity GetBy(string userName);
    }

    public interface IApiGetUserQuery
    {
        Model.UserIdentity GetBy(Guid apiKey);
    }
}
