namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
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