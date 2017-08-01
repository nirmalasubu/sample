using System;
using System.Collections.Generic;

namespace OnDemandTools.Web.Models.PathTranslation
{
    public class PathTranslationViewModel
    {
        public String Id { get; set; }
        public PathInfo Source { get; set; }
        public PathInfo Target { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}