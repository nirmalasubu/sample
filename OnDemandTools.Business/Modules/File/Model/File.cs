using System.Collections.Generic;
using System;

namespace OnDemandTools.Business.Modules.File.Model
{
    public class File
    {
        public File()
        {

        }
        
        public String Id { get; set; }
        public string MediaId { get; set; }
        public string AiringId { get; set; }
        public string ContentId { get; set; }
        public int? TitleId { get; set; }
        public string Type { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string AspectRatio { get; set; }
        public string Match { get; set; }

        public bool Video { get; set; }
        public bool Secure { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedDatetime { get; set; }
        public List<Content> Contents { get; set; }
      
        public bool Unique { get; set; }

    }

    public class Item
    {
        public Item(string id, string type, int position)
        {
            Id = id;
            Type = type;
            Position = position;
        }

        public int Position { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
    }


}
