using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace OnDemandTools.Web.Models.Destination
{
    public class Category
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public List<string> Brands { get; set; }

        public List<Title> Titles { get; set; }

        public List<int> TitleIds { get; set; }

        public List<int> SeriesIds { get; set; }
    }
}
