using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System.Linq;
using OnDemandTools.DAL.Modules.UserPermissions.Model;
using System;

namespace OnDemandTools.DAL.Modules.UserPermissions.Query
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
    }
}
