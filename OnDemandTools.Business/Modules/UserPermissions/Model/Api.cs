﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.UserPermissions.Model
{
    public class Api
    {
        public Api()
        {
            Claims = new List<string>();
            Destinations = new List<string>();
            Brands = new List<string>();
        }

        public bool IsActive { get; set; }

        public bool DestinationPermitAll { get; set; }

        public bool BrandPermitAll { get; set; }

        public Guid ApiKey { get; set; }

        public DateTime LastAccessTime { get; set; }

        public IEnumerable<string> Claims { get; set; }
        public IEnumerable<string> Destinations { get; set; }
        public IEnumerable<string> Brands { get; set; }

        public string TechnicalContactId { get; set; }
        public string FunctionalContactId { get; set; }
    }
}
