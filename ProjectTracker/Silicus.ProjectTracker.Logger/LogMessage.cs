using System;

namespace Silicus.ProjectTracker.Logger
{
    public class LogMessage
    {
        public int LogMessageId { get; set; }

        public string Message { get; set; }

        public DateTime RecordedAt { get; set; }

        public LogCategory Category { get; set; }

        public string ClassName { get; set; }
    }
}
