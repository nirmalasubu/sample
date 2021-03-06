﻿using System.Collections.Generic;
using System.Linq;
using System;

namespace OnDemandTools.DAL.Modules.Airings.Model
{
    public class ChangeNotification
    {
        public ChangeNotification()
        {
            ChangedProperties = new List<string>();
        }

        public string QueueName { get; set; }

        public string ChangeNotificationType { get; set; }

        public List<string> ChangedProperties { get; set; }

        public DateTime ChangedDateTime { get; set; }

        public bool ShouldSerializeChangedProperties()
        {
            return ChangedProperties.Any();
        }

    }
}
