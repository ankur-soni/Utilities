using System;

namespace Silicus.FrameworxProject.Models
{
    public class ProductBacklog
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string State { get; set; }

        public string Assignee { get; set; }
        public double TimeAllocated { get; set; }

        public double TimeSpent { get; set; }
    }
}
