using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Models.DataObjects
{
    public class EmployeeTitles
    {
        [Key]
        public int EmployeeTitlesId { get; set; }

        public int EmployeeId { get; set; }

        public int TitleId { get; set; }

        public bool IsCurrent { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }
    }
}