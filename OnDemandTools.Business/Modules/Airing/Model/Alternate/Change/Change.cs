namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Change
{
    public class Change
    {
        public string TheChange { get; set; }

        public ChangeValue Previous { get; set; }

        public ChangeValue Current { get; set; }
    }
}