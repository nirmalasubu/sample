namespace OnDemandTools.Business.Modules.UserPermissions.Model
{
    public class Permission
    {
        public Permission()
        {

        }

        public Permission(bool isAdmin)
        {
            CanAdd = isAdmin;
            CanEdit = isAdmin;
            CanRead = isAdmin;
            CanDelete = isAdmin;
        }

        public bool CanRead { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
