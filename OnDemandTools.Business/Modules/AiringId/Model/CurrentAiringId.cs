using System;
using OnDemandTools.Common.Model;

namespace OnDemandTools.Business.Modules.AiringId.Model
{
    public class CurrentAiringId : IModel
    {
        public CurrentAiringId()
        {
            // TODO: Add user identity
            //CreatedBy = WindowsIdentity.GetCurrent() == null ? "NA" : WindowsIdentity.GetCurrent().Name;
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