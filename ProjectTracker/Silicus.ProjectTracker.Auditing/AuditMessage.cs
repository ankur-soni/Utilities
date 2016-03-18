using System;

namespace Silicus.ProjectTracker.Auditing
{
    public class AuditMessage 
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        public string UserName { get; set; }
        public string OperationName { get; set; }

        /// <summary>
        /// Data could contain additional information about the operation 
        /// being performed e.g. RRID, USDOT. This value is extensibility
        /// point to allow any data to be stored. The data can be stored
        /// as JSON or XML.
        /// </summary>
        public string Data { get; set; }
    }
}
