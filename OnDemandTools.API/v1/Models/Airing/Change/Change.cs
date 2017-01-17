
namespace OnDemandTools.API.v1.Models.Airing.Change
{
    public class Change
    {
        public string TheChange { get; set; }

        public ChangeValue Previous { get; set; }

        public ChangeValue Current { get; set; }
    }
}
