﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.v1.Models.Product
{
    public class Tag
    {
        public Tag()
        {
           
        }

        public Tag(string tag)
        {
            Text = tag;
        }

        public string Text { get; set; }
    }
}
