using BLQueue= OnDemandTools.Business.Modules.Queue.Model;
using BLAiring= OnDemandTools.Business.Modules.Airing.Model;
using System;
using System.Linq;

namespace OnDemandTools.Business.Modules.AiringPublisher.Workflow
{
    public class MessagePriorityCalculator : IMessagePriorityCalculator
    {
        public byte? Calculate(BLQueue.Queue queue, BLAiring.Airing airing)
        {
            if (!queue.IsPriorityQueue) return null;

            if (airing.Flights.All(e => e.End < DateTime.UtcNow)) return 0;

            var firstAiringStartDate = airing.Flights.Select(e => e.Start).OrderBy(date => date).First();

            var differenceBetweenDates = (int)(firstAiringStartDate.Date - DateTime.UtcNow.Date).TotalDays;

            //Flight window already started 
            if (differenceBetweenDates < 0)
            {
                return 7;
            }
            if (differenceBetweenDates == 0) //Flight windows started today
            {
                return 6;
            }
            if (differenceBetweenDates == 1) //Flight Starts tomorrow
            {
                return 5;
            }
            if (differenceBetweenDates <= 3) //Flight starts with in next two days
            {
                return 4;
            }
            if (differenceBetweenDates <= 7) //Flight starts with in a week
            {
                return 3;
            }

            if(differenceBetweenDates <= 14)
            {
                return 2;
            }

            return 1;
        }
    }
}
