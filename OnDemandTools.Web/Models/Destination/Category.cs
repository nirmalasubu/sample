﻿using System;
using System.Collections.Generic;

namespace OnDemandTools.Web.Models.Destination
{
    public class Category
    {

        public Category()
        {
            Brands=new List<string>();
            Titles=new List<Title>();
            TitleIds=new List<int>();            
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Brands { get; set; }

        public List<Title> Titles { get; set; }

        public List<int> TitleIds { get; set; }
    }
}
