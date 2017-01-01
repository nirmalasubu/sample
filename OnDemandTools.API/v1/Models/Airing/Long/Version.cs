namespace OnDemandTools.API.v1.Models.Airing.Long
{
    public class Version
    {
        public Version()
        {
            ClosedCaptioning = new ClosedCaptioning();
        }

        public string ContentId { get; set; }
        public ClosedCaptioning ClosedCaptioning { get; set; }
    }    
}