namespace OnDemandTools.Web.Models.UserPermissions
{
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
