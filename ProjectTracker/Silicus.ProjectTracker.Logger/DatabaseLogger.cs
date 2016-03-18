using System;
using System.Configuration;
using System.Globalization;
using System.Threading;

namespace Silicus.ProjectTracker.Logger
{
    public class DatabaseLogger : ILogger, IDisposable
    {
        private static readonly Func<DateTime> DefaultDateGetter = () => DateTime.UtcNow;
        private readonly Func<DateTime> _dateGetter = DefaultDateGetter; 
        private bool _disposed;
        private IDataContext _dbContext;
        private IWindowsEventLogger _windowsEventLogger;
        private static readonly object lockInsert = new object();

        public DatabaseLogger(string connectionString, Type type = null, Func<DateTime> dateGetter = null)
            : this(connectionString, type)
        {
            if (dateGetter != null)
            {
                _dateGetter = dateGetter;                
            }
        }

        public DatabaseLogger(IDataContext dbContext, IWindowsEventLogger windowsEventLogger)
        {
            _dbContext = dbContext;
            _windowsEventLogger = windowsEventLogger;
        }

        internal DatabaseLogger(string connectionString, Type type)
        {
            try
            {
                _dbContext = new LoggerContext(connectionString);
                _windowsEventLogger = new WindowsEventLogger();
            }
            catch
            {
                // If anything goes wrong, DatabaseLogger will not throw any exception, just ignore it.
            }

            if (type != null)
            {
                ClassName = type.FullName;
            }
        }

        public string ClassName { get; set; }

        public void Log(string message, string sessionId = null)
        {
            LogInformation(LogCategory.Information, message, sessionId);
        }

        public void Log(string message, LogCategory category, string sessionId = null)
        {
            LogInformation(category, message, sessionId);
        }

        public void Log(Exception ex, string sessionId = null)
        {
            LogInformation(LogCategory.Error, ex.ToLoggableString(), sessionId);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    if (_dbContext != null)
                    {
                        _dbContext = null;
                    }
                }

                // Note disposing has been done.
                _disposed = true;
            }
        }

        private void LogInformation(LogCategory category, string message, string sessionId = null)
	    {
		    // Check if log message doesn't have lower level than what is configured for logging
			var configLogLevel = (int) Enum.Parse(typeof (LogCategory), ConfigurationManager.AppSettings["LogLevel"]);
		    if (configLogLevel <= (int)category)
		    {
			    try
			    {
			        lock (lockInsert)
			        {
			            var loggedDate = _dateGetter();
			            var messageWithThreadId =
			                "Thread Id: {0}, Message: {1}".FormatInvariant(Thread.CurrentThread.ManagedThreadId,
			                    message);

			            if (!string.IsNullOrEmpty(sessionId))
			                ClassName = sessionId;

			            var logInformation = new LogMessage
			            {
			                Message = messageWithThreadId,
			                Category = category,
			                RecordedAt = loggedDate,
			                ClassName = ClassName
			            };

			            _dbContext.Add<LogMessage>(logInformation);
			        }
			    }
			    catch (Exception e)
			    {
				    // If anything goes wrong, DatabaseLogger will not throw any exception, but create an entry in the windows events.
				    try
				    {
					    var exceptionMessage = e.Message;

					    if (e.InnerException != null)
					    {
						    exceptionMessage = ". Inner Exception Message: " + e.InnerException.Message;
					    }

					    _windowsEventLogger.WriteToEventLog(GetType().FullName,
						    Environment.CurrentManagedThreadId.ToString(CultureInfo.InvariantCulture), exceptionMessage);
				    }
				    catch (Exception)
				    {
					    // If anything goes wrong, DatabaseLogger will not throw any exception, just ignore it.
				    }
			    }
		    }
	    }
    }
}
