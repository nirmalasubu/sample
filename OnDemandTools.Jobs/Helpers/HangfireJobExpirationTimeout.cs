using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using OnDemandTools.Common.Configuration;
using System;

namespace OnDemandTools.Jobs.Helpers
{
    public class HangfireJobExpirationTimeout : JobFilterAttribute, IApplyStateFilter
    {
        AppSettings appSettings;
        public HangfireJobExpirationTimeout(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            //TODO change to FromDays after testing
            context.JobExpirationTimeout = TimeSpan.FromDays(appSettings.JobSchedules.JobLogExpirationTimeOutInDays);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {

        }
    }
}
