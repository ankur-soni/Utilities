using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class InfrastructureDetails : BaseEntity
    {
        public InfrastructureDetails()
        {
            IsActive = true;
        }

        [Key]
        public int InfrastructureDetailId { get; set; }

        public string DevelopmentAndQA { get; set; }

        public string UAT { get; set; }

        public string Production { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [Required]
        public int WeekId { get; set; }

        public bool IsActive { get; set; }
    }
}
