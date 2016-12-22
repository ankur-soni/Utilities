using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.FrameworxProject.Models
{
    public class FrameworxCategory
    {
        public FrameworxCategory()
        {
            Frameworxs = new List<Frameworx>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Frameworx> Frameworxs { get; set; }
    }
}
