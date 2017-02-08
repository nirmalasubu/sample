using System;
using System.Collections.Generic;

namespace OnDemandTools.Jobs.JobRegistry.Models.Model
{
    public class Content
    {
        public Content()
        {
            ContentId = String.Empty;
            MaterialIds = new List<string>();
        }

        public string ContentId { get; set; }

        public List<string> MaterialIds { get; set; }
    }
}