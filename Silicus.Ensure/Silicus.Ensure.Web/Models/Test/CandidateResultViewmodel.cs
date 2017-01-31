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
        [Required]
        public CandidateStatus Status { get; set; }
        [Required]
        public string ReviewerComment { get; set; }

        public int UserTestSuiteId { get; set; }
    }
}