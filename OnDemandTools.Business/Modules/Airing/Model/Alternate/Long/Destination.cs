﻿namespace OnDemandTools.Business.Modules.Airing.Model.Alternate.Long
{
    public class Destination
    {
        public int ExternalId { get; set; }

        public string Name { get; set; }

        public bool AuthenticationRequired { get; set; }

        public Package Package { get; set; }

        public Destination()
        {
            Package = new Package();
        }
    }    
}