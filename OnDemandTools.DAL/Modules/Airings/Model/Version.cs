namespace OnDemandTools.DAL.Modules.Airings.Model

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