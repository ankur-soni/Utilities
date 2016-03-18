using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Models.DataObjects
{
    public class SkillSet
    {
        public SkillSet()
        {
            Employees = new List<Employee>();
            Projects = new List<Project>();   
        }

        [Key]
        public int SkillSetId { get; set; }

        [Index("IX_Unique_SkillSet", 1, IsUnique = true)]
        [Required(ErrorMessage = "Name can't be blank")]
        [StringLength(30, ErrorMessage = "Name should contain less than 30 characters")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description should contain less than 200 characters")]
        public string Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
       
    }
}