using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Silicus.ProjectTracker.Logger
{
    [ExcludeFromCodeCoverage]
    public class WindowsEventLogger : IWindowsEventLogger
    {
        public void WriteToEventLog(string strLogName, string threadId, string strErrDetail)
        {
            var sqlEventLog = new EventLog();

            try
            {
                if (!EventLog.SourceExists(strLogName))
                {
                    CreateLog(strLogName);
                }

                sqlEventLog.Source = strLogName;
                sqlEventLog.WriteEntry("ThreadId = " + Convert.ToString(threadId) + "Message = " +
                    Convert.ToString(strErrDetail),
                    EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                sqlEventLog.Source = strLogName;
                sqlEventLog.WriteEntry(
                    Convert.ToString("INFORMATION: ") +
                    Convert.ToString(ex.Message),
                    EventLogEntryType.Information);
            }
            finally
            {
                sqlEventLog.Dispose();
            }
        }

        private bool CreateLog(string strLogName)
        {
            bool result = false;

            try
            {
                EventLog.CreateEventSource(strLogName, strLogName);
                var sqlEventLog = new EventLog { Source = strLogName, Log = strLogName };

                sqlEventLog.Source = strLogName;
                sqlEventLog.WriteEntry("The " + strLogName + " was successfully initialize component.", EventLogEntryType.Information);

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
