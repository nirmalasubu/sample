﻿using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Destination
{
    public class Category
    {
        public Category()
        {
            Brands = new List<string>();
            TitleIds = new List<int>();
        }

        public string Name { get; set; }

        public List<string> Brands { get; set; }

        public List<int> TitleIds { get; set; }
    }
}