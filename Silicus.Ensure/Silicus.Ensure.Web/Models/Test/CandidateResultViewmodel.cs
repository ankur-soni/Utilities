using Silicus.Ensure.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.Test
{
    public class CandidateResultViewmodel
    {
        public int CandidateUserId  { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public CandidateStatus Status { get; set; }
        [Required(ErrorMessage = "Reviewer comment is required.")]
        [Display(Name ="Reviewer comment")]
        public string ReviewerComment { get; set; }

        public int UserTestSuiteId { get; set; }
    }
}