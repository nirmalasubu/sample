using System;

namespace OnDemandTools.API.v1.Models.Airing.Update
{
    public class Product
    {
        public Guid ExternalId { get; set; }

        public bool IsAuth { get; set; }
    }
}