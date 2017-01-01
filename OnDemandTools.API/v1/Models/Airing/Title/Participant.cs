using System;

namespace OnDemandTools.API.v1.Models.Airing.Title
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
