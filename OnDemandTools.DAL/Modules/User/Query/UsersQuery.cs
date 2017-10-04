using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;

namespace OnDemandTools.DAL.Modules.User.Queries
{
    public class UsersQuery : IGetUsersQuery, IApiGetUserQuery
    {
        private readonly MongoDatabase _database;

        public UsersQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public UsersQuery(MongoDatabase db)
        {
            _database = db;
        }

        public IQueryable<Model.UserIdentity> GetUsers()
        {
            var users = _database.GetCollection<Model.UserIdentity>("UserIdentity").AsQueryable();

            return users.AsQueryable();
        }

        public Model.UserIdentity GetById(String id)
        {

            var users = _database.GetCollection<Model.UserIdentity>("UserIdentity").AsQueryable();

            var user = users.FirstOrDefault(a => a.Id == ObjectId.Parse(id));

            return user ?? new Model.UserIdentity();
        }

        public Model.UserIdentity GetBy(string userName)
        {
            var users = _database.GetCollection<Model.UserIdentity>("UserIdentity").AsQueryable();

            var user = users.FirstOrDefault(a => a.UserName.ToUpper() == userName.ToUpper()) ??
                       users.FirstOrDefault(a => a.UserName.ToUpper() == Model.GuestUser.Name.ToUpper());

            return user ?? new Model.UserIdentity();
        }

        public Model.UserIdentity GetBy(Guid apiKey)
        {
            var users = _database.GetCollection<Model.UserIdentity>("UserIdentity").AsQueryable();

            var user = users.FirstOrDefault(a => a.ApiKey == apiKey);

            return user ?? new Model.UserIdentity();
        }
        
    }
}
