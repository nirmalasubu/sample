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

        public void Start(int intervalInSeconds)
        {
            try
            {
                _intervalInMilliseconds = intervalInSeconds * 60;
                _timer = new Timer(Callback, null, _intervalInMilliseconds, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void Callback(Object state)
        {
            try
            {
                ViewRefresher _viewRefresher = new ViewRefresher();
                _viewRefresher.Refresh();
                _timer.Change(_intervalInMilliseconds, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                
            }



        }
    }
}
