using OnDemandTools.Common.Model;
using System;

namespace OnDemandTools.Web.Models.Status
{
    public class StatusModel : IModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string User { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
