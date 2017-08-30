
using MongoDB.Bson.Serialization.Attributes;

namespace OnDemandTools.DAL.Modules.UserPermissions.Model
{
    [BsonIgnoreExtraElements]
    public class PortalModule
    {
        public string ModuleName { get; set; }

        public string ModuleDisplayName { get; set; }

        public string ModuleType { get; set; }

        public float DisplayOrder { get; set; }

        public Permission ModulePermission { get; set; }
    }
}
