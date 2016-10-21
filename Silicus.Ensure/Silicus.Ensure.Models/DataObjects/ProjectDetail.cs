using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class ProjectDetail
    {
        [ScaffoldColumn(false)]
        public int ProjectDetailId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        [Required]
        [MaxLength(5)]
        public string Status { get; set; }
    }
}
