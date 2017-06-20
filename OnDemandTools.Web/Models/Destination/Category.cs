﻿using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Destination
{
    public class Category
    {
        public Category()
        {
            Titles = new List<Title>();
          
        }
        public string Name { get; set; }

        public List<string> Brands { get; set; }

        public List<Title> Titles { get; set; }

        public List<int> TitleIds { get; set; }

        public List<int> SeriesIds { get; set; }
    }
}
