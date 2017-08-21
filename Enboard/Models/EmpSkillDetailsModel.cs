using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace Models
{
  public class EmpSkillDetailsModel
    {
        public long Id { get; set; }
        public Nullable<long> UserId { get; set; }

        [Display(Name = "Skill Name")]
        public string Skill { get; set; }

        [Display(Name = "Skill Name")]
       
        public int SkillId { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Please select Skill")]
        [Display(Name = "Skill Name")]
        public int Skill_Id { get; set; }
        public string OtherSkill { get; set; }

        [Display(Name = "Experience in Years")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Please select Year")]
        public int ExprInYears { get; set; }

        [Display(Name = "Experience in Months")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Please select Month")]
        public int ExprInMonths { get; set; }

        public IPagedList<EmpSkillDetailsHistory> EmpSkillDetailsList { get; set; }
       
        public Nullable<bool> IsActive { get; set; }

        public ProfessionalDetailsModel ProfessionalDetailsModel { get; set; }
    }

  public class EmpSkillDetailsHistory
  {
      public long Id { get; set; }

      public Nullable<long> UserId { get; set; }

      public string Skill { get; set; }

      public string Skill_Experience { get; set; }     

      public string OtherSkill { get; set; }

      public string SkillName { get; set; }

      public Nullable<bool> IsActive { get; set; }
     
      
  }
 
}
