using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.Finder.Models.DataObjects
{
    public class Project
    {
        public Project()
        {
            Employees = new List<Employee>();
            SkillSets = new List<SkillSet>();
        }
        
        [Key]
        [Display(Name = "Project Id")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project Name can't be blank")]
        [StringLength(100, ErrorMessage = "Project Name  should contain less than 100 characters")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Index("IX_Unique_ProjectCode",1,IsUnique = true)]
        [Display(Name = "Project Code")]
        [StringLength(10, ErrorMessage = "Project Code should contain less than 10 characters")]
        public string ProjectCode { get; set; }
        
        [StringLength(500, ErrorMessage = "Description should contain less than 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select Project Type")]
        [Display(Name = "Project Type")]
        public ProjectType? ProjectType { get; set; }

        [Required(ErrorMessage = "Please select Engagement Type")]
        [Display(Name = "Engagement Type")]
        public EngagementType? EngagementType { get; set; }

        public Status? Status { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Expected End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpectedEndDate { get; set; }

        [Display(Name = "Actual End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ActualEndDate { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ArchiveDate { get; set; }

        [Display(Name = "Engagement Manager")]
        //[Required(ErrorMessage = "Please Select Engagement Manager")]
        public int? EngagementManagerId { get; set; }

        [Display(Name = "Project Manager")]
        [Required(ErrorMessage = "Please Select Project Manager")]
        public int? ProjectManagerId { get; set; }

        [StringLength(250, ErrorMessage = "Additional Notes should contain less than 250 characters")]
        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }

        [NotMapped]
        public int[] skillSetId { get; set; }
        public virtual ICollection<SkillSet> SkillSets { get; set; }

        [NotMapped]
        public int[] EmployeeIds { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}