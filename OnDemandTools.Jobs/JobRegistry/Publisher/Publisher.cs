using System;
using System.Threading;

namespace OnDemandTools.Jobs.JobRegistry.Publisher
{
    public class Publisher
    {
               
        public void Execute()
        {
            var rand = new Random(1);
            var min = rand.Next(3, 10);
            min = min * 60 * 1000;
            Thread.Sleep(min);
        }
    }
}
