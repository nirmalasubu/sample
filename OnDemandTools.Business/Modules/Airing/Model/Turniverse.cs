﻿using System;
using System.Collections.Generic;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class Turniverse
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public IList<Feed> Feeds { get; set; }
    }
}