﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.API.Tests.Helpers
{
    public class OrderAttribute : Attribute
    {
        public int I { get; }

        public OrderAttribute(int i)
        {
            I = i;
        }
    }
}
