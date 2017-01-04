namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class Destination
    {
        public int ExternalId { get; set; }

        public string Name { get; set; }

        public bool AuthenticationRequired { get; set; }

        public Package Package { get; set; }

        public Destination()
        {
            Package = new Package();
        }
    }
}