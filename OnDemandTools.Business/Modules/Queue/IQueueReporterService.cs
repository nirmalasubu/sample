﻿namespace OnDemandTools.Business.Modules.Queue
{
    public interface IQueueReporterService
    {
        void Report(Model.Queue queue, string airingId, bool isActiveAiringStatus, string message, int statusEnum, bool unique = false);

        void BimReport(Model.Queue queue, string airingId, bool isActiveAiringStatus, string message, int statusEnum);
    }
}
