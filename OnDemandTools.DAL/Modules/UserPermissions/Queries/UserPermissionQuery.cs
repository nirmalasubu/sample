using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System.Linq;
using OnDemandTools.DAL.Modules.UserPermissions.Model;
using System;
using MongoDB.Driver.Builders;

namespace OnDemandTools.DAL.Modules.UserPermissions.Queries
{
    public class UserPermissionQuery : IUserPermissionQuery
    {
        private readonly MongoDatabase _database;

        public UserPermissionQuery(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        public IQueryable<Model.UserPermission> Get()
        {
            var userPermission = _database
                .GetCollection<Model.UserPermission>("UserPermission")
                .AsQueryable();

            return userPermission.AsQueryable();
        }

        public IQueryable<PortalModule> GetAllPortalModules()
        {
            var modules = _database
                .GetCollection<Model.PortalModule>("PortalModules")
                .AsQueryable();

            return modules.AsQueryable();
        }

        public Model.UserPermission GetById(string objectId)
        {
            return _database
             .GetCollection<Model.UserPermission>("UserPermission").AsQueryable()
             .FirstOrDefault(e => e.Id == new ObjectId(objectId));
        }

        public IQueryable<UserPermission> GetContactForByUserId(string id)
        {

            var userPermissionCollection = _database.GetCollection<UserPermission>("UserPermission");

            // Find TechnicalContactId and FunctionalContactId with the corresponding id
            var userPermissionQuery = Query.Or(Query.EQ("Api.TechnicalContactId", id), Query.EQ("Api.FunctionalContactId", id));

            return userPermissionCollection.Find(userPermissionQuery).AsQueryable();
        }
    }
}
