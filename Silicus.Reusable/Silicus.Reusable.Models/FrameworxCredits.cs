using System.ComponentModel.DataAnnotations;

namespace Silicus.FrameworxProject.Models
{
    public class FrameworxCredits
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int FrameworxId { get; set; }

        public virtual Frameworx Frameworx { get; set; }
    }
}
