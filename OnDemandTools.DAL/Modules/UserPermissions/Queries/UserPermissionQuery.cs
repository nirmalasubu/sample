using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnDemandTools.DAL.Database;
using System.Linq;
using OnDemandTools.DAL.Modules.UserPermissions.Model;
using System;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;

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

        public UserPermission GetByApiKey(Guid apiKey)
        {
            return _database
             .GetCollection<Model.UserPermission>("UserPermission").AsQueryable()
             .FirstOrDefault(e => e.Api.ApiKey == apiKey);
        }

        public UserPermission GetByApiKeyAndUpdateLastAccessedTime(Guid apiKey)
        {
            var userPermissions = _database.GetCollection<UserPermission>("UserPermission");

            var portalUserCheck = Query.Or(Query.NE("UserType", UserType.Api), Query.EQ("Portal.IsActive", true));

            var query = Query.And(Query.EQ("Api.ApiKey", apiKey), Query.EQ("Api.IsActive", true), portalUserCheck);

            var findAndModifyResult = userPermissions.FindAndModify(new FindAndModifyArgs()
            {
                Query = query,
                Update = Update.Set("Api.LastAccessTime", DateTime.UtcNow),
                VersionReturned = FindAndModifyDocumentVersion.Modified
            });

            return findAndModifyResult.GetModifiedDocumentAs<UserPermission>();
        }

        public Model.UserPermission GetById(string objectId)
        {
            return _database
             .GetCollection<Model.UserPermission>("UserPermission").AsQueryable()
             .FirstOrDefault(e => e.Id == new ObjectId(objectId));
        }

        public Model.UserPermission GetByUserName(string emailAddress)
        {
            return _database
             .GetCollection<Model.UserPermission>("UserPermission").AsQueryable()
             .FirstOrDefault(e => e.UserName.ToLower() == emailAddress.ToLower());
        }

        public UserPermission GetByUserNameAndUpdateLastLoginTime(string emailAddress)
        {
            var userPermissions = _database.GetCollection<UserPermission>("UserPermission");

            var query = Query.And(Query.Matches("UserName", BsonRegularExpression.Create(new Regex(emailAddress, RegexOptions.IgnoreCase)))
                , Query.EQ("Portal.IsActive", true));

            var findAndModifyResult = userPermissions.FindAndModify(new FindAndModifyArgs()
            {
                Query = query,
                Update = Update.Set("Portal.LastLoginTime", DateTime.UtcNow),
                VersionReturned = FindAndModifyDocumentVersion.Modified
            });

            return findAndModifyResult.GetModifiedDocumentAs<UserPermission>();
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
