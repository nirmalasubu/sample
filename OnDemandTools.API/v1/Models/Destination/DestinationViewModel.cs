using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OnDemandTools.API.v1.Models.Destination
{
    public class DestinationViewModel : IModel
    {
        public DestinationViewModel()
        {
            Deliverables = new List<DeliverableViewModel>();
            Properties = new List<PropertyViewModel>();
            Playlists = new List<Playlist>();

            this.UpdateCreatedBy();
        }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PropertyViewModel> Properties { get; set; }

        public List<DeliverableViewModel> Deliverables { get; set; }

        public ContentViewModel Content { get; set; }

        public List<Playlist> Playlists { get; set; }

        public bool AuditDelivery { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }

    public class Playlist
    {
        Playlist()
        {
        }

        public Playlist(string type)
        {
            Type = type;
            Items = new List<Item>();
        }

        public string Type { get; set; }


        public List<Item> Items { get; set; }
    }

    [XmlType(Namespace = "http://api.odt.turner.com/schemas/airing/options/Destination", TypeName = "PlaylistItem")]
    public class Item
    {
        Item()
        {
        }

        public Item(string id, string type, int position, int length)
        {
            Id = id;
            Type = type;
            Position = position;
            Length = length;
        }

        public int Position { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
    }
}
