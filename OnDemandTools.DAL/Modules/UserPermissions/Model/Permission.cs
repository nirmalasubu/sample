using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.UserPermissions.Model
{
    [BsonIgnoreExtraElements]
    public class Permission
    {
        public Permission()
        {

        }

        public bool CanRead { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set;  }

        public bool CanDelete { get; set; }
    }
}
