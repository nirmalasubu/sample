using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System.Linq;

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
            var destinations = _database
                .GetCollection<Model.UserPermission>("UserPermission")
                .AsQueryable();

            return destinations.AsQueryable();
        }

        public Model.UserPermission GetById(string objectId)
        {
            return _database
             .GetCollection<Model.UserPermission>("UserPermission").AsQueryable()
             .FirstOrDefault(e => e.Id == new ObjectId(objectId));
        }
    }
}
