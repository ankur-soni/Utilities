using System;

namespace Silicus.Finder.Models
{
    public class NotificationMessage
    {
        public int Id { get; set; }
        public Guid NotificationId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
