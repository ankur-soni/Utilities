using System;
using System.Configuration;
using System.Threading;
using Eda.RDBI.Logger;

namespace Eda.RDBI.Services.NotificationFeed
{
    public class FeedSchedular : IFeedSchedular, IDisposable
    {
        private readonly ILogger _logger;
        readonly IFeedProcessor _feedProcessor;
        AutoResetEvent _autoEvent;
        TimerCallback _timerCallBack;
        Timer _timerThread;
        private const int PollDefaultTime = 1*60*60*1000; // 1 Hour default

        public FeedSchedular(ILogger logger, IFeedProcessor feedProcessor)
        {
            _logger = logger;
            _feedProcessor = feedProcessor;
        }

        public void Start()
        {
            try
            {
                _autoEvent = new AutoResetEvent(false);
                var pollTimeFromConfig = ConfigurationManager.AppSettings["FeedPollTimerDuration"];
                long timePeriod = string.IsNullOrEmpty(pollTimeFromConfig) ? PollDefaultTime : Convert.ToInt64(ConfigurationManager.AppSettings["FeedPollTimerDuration"]);

                _timerCallBack = Schedule;

                _timerThread = new Timer(_timerCallBack, _autoEvent, 0, timePeriod);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void Stop()
        {
            try
            {
                StopTimer();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void Dispose()
        {
            StopTimer();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timerThread != null)
                {
                    _timerThread.Dispose();
                    _timerThread = null;
                }
            }
        }

        private void Schedule(object stateInfo)
        {
            var autoResetEvent = (AutoResetEvent)stateInfo;
            
            _feedProcessor.ProcessFeed();

            autoResetEvent.Set();
        }
        
        private void StopTimer()
        {
            if (_timerThread != null)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void LogException(Exception exception)
        {
            _logger.Log(exception);
            System.Diagnostics.Trace.WriteLine(exception.Message);
        }
    }
}