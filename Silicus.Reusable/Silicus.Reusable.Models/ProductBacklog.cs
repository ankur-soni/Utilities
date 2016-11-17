using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Silicus.FrameworxProject.Models
{
    public class ProductBacklog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string AssignedTo { get; set; }

        public string Owener { get; set; }

        public string ReceivedBy { get; set; }

        public int Age { get; set; }
    }
}
