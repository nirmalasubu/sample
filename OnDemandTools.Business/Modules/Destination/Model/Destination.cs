﻿
using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Destination.Model
{    
    public class Destination:IModel
    {
        public Destination()
        {
            Deliverables = new List<Deliverable>();
            Properties = new List<Property>();
            Categories = new List<Category>();
        }

        public String Id { get; set; }

        public int ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Property> Properties { get; set; }

        public List<Deliverable> Deliverables { get; set; }

        public List<Category> Categories { get; set; }

        public Content Content { get; set; }

        public bool AuditDelivery { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
