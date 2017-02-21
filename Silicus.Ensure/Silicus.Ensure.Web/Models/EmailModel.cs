using System;
using System.ComponentModel.DataAnnotations;
using System.IO;


namespace Silicus.Ensure.Web.Models
{
    [Serializable]
    public class EmailModel
    {
        [Required]
        //[Display(Name = "Enter Name")]
        public string Name { get; set; }

        [Required]
        //[Display(Name = "Enter Email")]
        public string Email { get; set; }


        public string CandidateName { get; set; }

        public string CandidateStatus { get; set; }

        public string RecruiterName { get; set; }
    }
}