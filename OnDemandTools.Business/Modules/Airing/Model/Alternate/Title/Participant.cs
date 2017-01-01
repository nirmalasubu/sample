using System;

namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Title
{
    public class Participant
    {
        public bool IsKey { get; set; }

        public String RoleType { get; set; }

        public bool IsOnScreen { get; set; }

        public String ParticipantType { get; set; }

        public string Name { get; set; }

        public int ParticipantId { get; set; }
    }
}
