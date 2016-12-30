using System;

namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class Status
    {
        public int StatusEnum { get; set; }
        public int ReporterEnum { get; set; }
        public int DestinationEnum { get; set; }
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}