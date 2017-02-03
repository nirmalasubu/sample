using OnDemandTools.Business.Modules.Airing;
using System;
using System.Threading;


namespace OnDemandTools.Jobs.JobRegistry.TitleSync
{
    public class TitleSync
    {
        IAiringService svc;
        public TitleSync(IAiringService svc)
        {
           this.svc = svc;
        }

        public void Execute()
        {
            var rand = new Random(1);
            var min = rand.Next(3, 10);
            min = min * 60 * 1000;
            Thread.Sleep(min);
        }
    }
}
