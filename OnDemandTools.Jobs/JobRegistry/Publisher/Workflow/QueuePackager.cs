﻿using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.Jobs.Models;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class QueuePackager : IPackager
    {
        public QueueAiring Package(Airing airing, Action action)
        {
            var queueAiring = new QueueAiring();
            queueAiring.AiringId = airing.AssetId;
            queueAiring.Action = action.ToString();
            return queueAiring;
        }
    }
}