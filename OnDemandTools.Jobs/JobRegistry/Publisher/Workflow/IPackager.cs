﻿using OnDemandTools.Business.Modules.Airing.Model;
using OnDemandTools.DAL.Modules.Airings.Model.Queues;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public interface IPackager
    {
        QueueAiring Package(Airing airing, Action action);
    }
}