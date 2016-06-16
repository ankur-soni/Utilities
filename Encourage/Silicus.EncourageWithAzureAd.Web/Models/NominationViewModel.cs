using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class NominationViewModel
    {
        [Required(ErrorMessage = "Select Award")]
        public int AwardId { get; set; }

        public int NominationId { get; set; }
        
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "Select Resource")]
        public int ResourceId { get; set; }

        [Required(ErrorMessage = "Select Project")]
        public int? ProjectID { get; set; }

        [Required(ErrorMessage = "Select Department")]
        public int? DepartmentId { get; set; }

        public bool IsPLC { get; set; }

        public bool? IsSubmitted { get; set; }

        public string MainComment { get; set; }

        [Required(ErrorMessage = "Select Project or Department")]
        public string SelectResourcesBy { get; set; }  //from project or from department

        [Required(ErrorMessage = "Please provide comment against criteria")]
        public IList<CriteriaCommentViewModel> Comments { get; set; }

        public NominationViewModel()
        {
            Comments = new List<CriteriaCommentViewModel>();
        }

    }
}