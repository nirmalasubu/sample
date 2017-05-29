using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Destination
{
    public class Content
    {
        public bool HighDefinition { get; set; }
        public bool StandardDefinition { get; set; }
        public bool Cx { get; set; }
        public bool NonCx { get; set; }
    }
}
