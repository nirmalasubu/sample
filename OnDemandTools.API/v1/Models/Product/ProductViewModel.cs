﻿using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Product
{
    public class ProductViewModel : IModel
    {
        public ProductViewModel()
        {
            Destinations = new List<string>();
            Tags = new List<TagViewModel>();

            this.UpdateCreatedBy();
        }

        public string Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MappingId { get; set; }

        public List<TagViewModel> Tags { get; set; }

        public List<string> Destinations { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}