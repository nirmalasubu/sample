using System;

namespace OnDemandTools.DAL.Modules.Airings.Model
{
    public class Task
    {
        public string Name { get; set; }
        public string State { get; set; }
        public DateTime ReportedAt { get; set; }
    }
}