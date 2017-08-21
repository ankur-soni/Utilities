using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Mvc;

namespace Models
{
    public class FinalSubmitViewModel
    {
        public string PersonalDetailsError { get; set; }
        public string ContactDetailsError { get; set; }
        public string EducationDetailsError { get; set; }
        public string EmploymentDetailsError { get; set; }
        public string FamilyDetailsError { get; set; }
        public string UploadDetailsError { get; set; }

        [Required]
        public bool FinalStatus { get; set; } 
    }
}
