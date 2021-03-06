﻿using OnDemandTools.Web.Models.DeliveryQueue;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnDemandTools.Web.SignalR
{
    public class AutomaticViewRefresher
    {
        private Timer _timer;
        private int _intervalInMilliseconds;
        public readonly IDeliveryQueueData viewformatter;
        ILogger _logger;
        public AutomaticViewRefresher(IDeliveryQueueData viewformatter, ILogger logger)
        {
            this.viewformatter = viewformatter;
            _logger = logger;
        }

        public void Start(int intervalInSeconds)
        {
            try
            {
                _intervalInMilliseconds = intervalInSeconds * 60;
                _timer = new Timer(Callback, null, _intervalInMilliseconds, Timeout.Infinite);               
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in loading signalR ");
                throw ex;
            }
        }


        private void Callback(Object state)
        {
            try
            {
                ViewRefresher _viewRefresher = new ViewRefresher();
                QueuesHubModel dataToBroadCast = viewformatter.FetchQueueDeliveryCounts();
                _viewRefresher.Refresh(dataToBroadCast);
                _timer.Change(_intervalInMilliseconds, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                throw ex;  
            }



        }
    }
}
