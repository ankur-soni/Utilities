using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.JobVite
{
    public class AssignTestViewModel
    {
        [Required(ErrorMessage ="Requisition is required")]
        public int RequisitionId { get; set; }

        [Required(ErrorMessage = "TestSuite is required")]
        public int TestSuiteId { get; set; }

        [Required(ErrorMessage = "Candidate is required")]
        public List<string> CandidatesJson { get; set; }
    }
}