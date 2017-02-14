using System;

namespace OnDemandTools.Business.Modules.Job.Model
{
    public class TitleJobModel
    {
        public DateTime LastRunDateTime { get; set; }
        public string JobName { get; set; }      
        public DateTime CreateDateTime { get; set; }  
        public string LastProcessedTitleBSONId { get; set; }
    }
}
