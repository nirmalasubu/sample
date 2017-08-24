using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDemandTools.DAL.Modules.UserPermissions.Model;
using MongoDB.Driver;
using OnDemandTools.DAL.Database;

namespace OnDemandTools.DAL.Modules.UserPermissions.Command
{
    public class UserPermissionCommand : IUserPermissionCommand
    {
        private readonly MongoDatabase _database;

        public UserPermissionCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }
        public UserPermission Save(UserPermission userPermission)
        {
            var collection = _database.GetCollection<UserPermission>("UserPermission");

            collection.Save(userPermission);

            return userPermission;
        }
    }
}
