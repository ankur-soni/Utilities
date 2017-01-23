using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Models
{
    public class NominationViewModel
    {
        [Required(ErrorMessage = "Select Nomination Category")]
        public int AwardId { get; set; }
        public int NominationId { get; set; }
        public int ManagerId { get; set; }
        [Required(ErrorMessage = "Select Employee")]
        public int ResourceId { get; set; }
        [Required(ErrorMessage = "Select Project")]
        public int? ProjectID { get; set; }
        [Required(ErrorMessage = "Select Department")]
        public int? DepartmentId { get; set; }
        [Required(ErrorMessage = "Please mention the reason")]
        [MaxLength(500, ErrorMessage = "Reason should not exceed 500 letters.")]
        public string OtherNominationReason { get; set; }
        public bool IsOther { get; set; }
        public bool IsLocked { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool IsHistorical { get; set; }
        [Required(ErrorMessage = "Please provide Comment")]
        public string MainComment { get; set; }
        [Required(ErrorMessage = "Select Project or Department")]
        public string SelectResourcesBy { get; set; }  //from project or from department
        [Required(ErrorMessage = "Please provide comment against criteria")]
        public IList<CriteriaCommentViewModel> Comments { get; set; }
        public SelectList ListOfAwards { get; set; }
        public SelectList ProjectsUnderCurrentUser { get; set; }
        public SelectList DepartmentsUnderCurrentUser { get; set; }
        public SelectList Resources { get; set; }
        public DateTime CustomDate { get; set; }

        #region For Displaying on Edit Nomination
        public string ResourceName { get; set; }
        public string AwardName { get; set; }
        public string ProjectOrDeptName { get; set; }
        #endregion For Displaying on Edit Nomination


        public NominationViewModel()
        {
            Comments = new List<CriteriaCommentViewModel>();
        }

    }
}