using Silicus.FrameworxProject.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Silicus.FrameworxProject.Models
{
    public class FrameworxFeedback
    {
        [Key]
        public int Id { get; set; }
        public int FrameworxId { get; set; }

        public int UserId { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string FeedBackFor { get; set; }

        public DateTime LastChange { get; set; }

        public virtual Frameworx Frameworx { get; set; }
    }
}
