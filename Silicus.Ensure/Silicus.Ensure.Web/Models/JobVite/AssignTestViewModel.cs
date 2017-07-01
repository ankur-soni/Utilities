using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models.JobVite
{
    public class AssignTestViewModel
    {
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidatePosition { get; set; }
        public string CandidateJobViteId { get; set; }
        public int existingAssignedTest { get; set; }

        //[Required(ErrorMessage ="Requisition is required")]
        //public int RequisitionId { get; set; }

        [Required(ErrorMessage = "TestSuite is required")]
        public int TestSuiteId { get; set; }

        [Required(ErrorMessage = "Reviewer is required")]
        public int ReviewerId { get; set; }
        //[Required(ErrorMessage = "Candidate is required")]
        //public List<string> CandidatesJson { get; set; }
    }
}