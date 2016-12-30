namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class PlayItem
    {
        public PlayItem() { }

        public PlayItem(string id, string type, int position)
        {
            Id = id;
            Type = type;
            Position = position;
        }

        public int Position { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }

        public string ItemType { get; set; }
        public string IdType { get; set; }

    }
}