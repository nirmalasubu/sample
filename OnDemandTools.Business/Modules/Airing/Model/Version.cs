namespace OnDemandTools.Business.Modules.Airing.Model

{
    public class Version
    {
        public Version()
        {
            ClosedCaptioning = new ClosedCaptioning();
        }

        public string Source { get; set; }
        public string ContentId { get; set; }
        public ClosedCaptioning ClosedCaptioning { get; set; }
    }    
}