using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Web.Models
{
    public class NominationViewModel
    {
        [Required(ErrorMessage = "Select Award")]
        public int AwardId { get; set; }

        public int ManagerId { get; set; }
        public int ResourceId { get; set; }
        public int? ProjectID { get; set; }
        public int? DepartmentId { get; set; }
        public bool? IsPLC { get; set; }
        public bool? IsSubmitted { get; set; }

        [Required(ErrorMessage = "Select Project or Department")]
        public string SelectResourcesBy { get; set; }  //from project or from department
        public IList<CriteriaCommentViewModel> Comments { get; set; }

        public NominationViewModel()
        {
            Comments = new List<CriteriaCommentViewModel>();   
        }
    
    }
}