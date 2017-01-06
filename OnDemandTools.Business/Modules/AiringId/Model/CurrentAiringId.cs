using System;
using OnDemandTools.Common.Model;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Modules.AiringId.Model
{
    public class CurrentAiringId : IModel
    {
        public CurrentAiringId()
        {
            CreatedDateTime = DateTime.UtcNow;

        }

        public String Id { get; set; }
        public string AiringId { get; set; }
        public string Prefix { get; set; }
        public int SequenceNumber { get; set; }
        public BillingNumber BillingNumber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public Boolean Locked { get; set; }
    }
}