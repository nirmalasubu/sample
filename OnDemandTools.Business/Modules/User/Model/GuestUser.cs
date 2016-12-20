namespace OnDemandTools.Business.Modules.User.Model
{
    public static class GuestUser
    {
        public static string Name { get; set; }

        static GuestUser()
        {
            Name = "guest";
        }
    }
}
