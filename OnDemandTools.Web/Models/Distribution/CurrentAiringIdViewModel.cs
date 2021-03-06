﻿using OnDemandTools.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Web.Models.Distribution
{
    public class CurrentAiringIdViewModel : IModel
    {
        public CurrentAiringIdViewModel()
        {

        }

        public string Id { get; set; }
        public string AiringId { get; set; }
        public string Prefix { get; set; }
        public int SequenceNumber { get; set; }
        public int BillingNumberLower { get; set; }
        public int BillingNumberCurrent { get; set; }
        public int BillingNumberUpper { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
