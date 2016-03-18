using System;

namespace Silicus.ProjectTracker.Logger
{
    public interface ILogger
    {
        void Log(string message, string sessionId = null);

        void Log(string message, LogCategory category, string sessionId = null);

        void Log(Exception ex, string sessionId = null);
    }
}