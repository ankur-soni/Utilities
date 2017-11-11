using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
  public  class ProfessionalDetailsModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        
       // [Required(ErrorMessage="Please enter Skill")]
        public string SkillSet { get; set; }
        
       // [Required(ErrorMessage = "Please enter Experience")]
        public string Experience { get; set; }
        
        public string Certification { get; set; }
        
        public string OtherCertification { get; set; }

        [Required(ErrorMessage = "Please select Year")]
        public string ExprInYears { get; set; }

        [Required(ErrorMessage = "please select Month")]
        public string ExprInMonths {get; set;}
    }
}
