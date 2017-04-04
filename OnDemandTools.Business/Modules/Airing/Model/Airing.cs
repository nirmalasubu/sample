using System;
using System.Collections.Generic;


namespace OnDemandTools.Business.Modules.Airing.Model
{
    public class Airing
    {
        public virtual String Id { get; set; }

        public virtual string AssetId { get; set; }

        public virtual string MediaId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        public virtual string Network { get; set; }

        public virtual string Platform { get; set; }

        public IList<AiringLink> Airings { get; set; }

        public Duration Duration { get; set; }

        public Title Title { get; set; }

        public Turniverse Turniverse { get; set; }

        public IList<Flight> Flights { get; set; }

        public Flags Flags { get; set; }

        public IList<Version> Versions { get; set; }

        public IList<PlayItem> PlayList { get; set; }

        public IList<string> DeliveredTo { get; set; }

        public IList<string> DeviceExclusions { get; set; }

        public IList<string> WebFlags { get; set; }

        public ISet<string> Tasks { get; set; }

        public virtual Guid ReleaseId { get; set; }

        public virtual DateTime ReleaseOn { get; set; }

        public virtual string ReleaseBy { get; set; }

        public bool DisableTracking { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public Dictionary<string, bool> Status { get; set; }

        public IList<string> IgnoredQueues { get; set; }

        public List<Alternate.Title.Title> FlowTitleData { get; set; }

        public List<ChangeNotification> ChangeNotifications { get; set; }

        public Airing()
        {
            Airings = new List<AiringLink>();
            Flights = new List<Flight>();
            Versions = new List<Version>();
            PlayList = new List<PlayItem>();
            DeviceExclusions = new List<string>();
            WebFlags = new List<string>();
            FlowTitleData=new List<Alternate.Title.Title>();
            Duration = new Duration();
            Title = new Title();
            Flags = new Flags();
            DeliveredTo = new List<string>();
            IgnoredQueues = new List<string>();
            Properties = new Dictionary<string, object>();
            Status = new Dictionary<string, bool>();
            ChangeNotifications = new List<ChangeNotification>();
        }
    }
}