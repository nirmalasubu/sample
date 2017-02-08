
namespace OnDemandTools.Jobs.JobRegistry.Publisher.Validating.Validators
{
    public class BimContent
    {
        public BimContent(string id, string type, string source)
        {
            Id = id;
            Type = type;
            Source = string.IsNullOrWhiteSpace(source) ? Sources.Orion.ToString() : source;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
    }
}