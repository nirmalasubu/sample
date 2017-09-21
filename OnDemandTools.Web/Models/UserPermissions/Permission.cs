namespace OnDemandTools.Web.Models.UserPermissions
{
    public class Permission
    {
        public Permission()
        {

        }

        public Permission(bool isAdmin)
        {
            CanAdd = isAdmin;
            CanRead = isAdmin;
            CanEdit = isAdmin;
            CanDelete = isAdmin;
        }

        public bool CanAddOrEdit
        {
            get
            {
                return CanAdd || CanEdit;
            }
        }

        public bool DisableControl
        {
            get
            {
                return CanRead && !CanAdd && !CanEdit;
            }
        }

        public bool CanRead { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
