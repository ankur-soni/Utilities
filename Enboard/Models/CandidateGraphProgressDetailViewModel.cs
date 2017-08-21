using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CandidateGraphProgressDetailViewModel
    {
        public double? PersonalDetailsPercentage { get; set; }
        public double? ContactDetailsPercentage { get; set; }
        public double? EducationDetailsPercentage { get; set; }
        public double? EmploymentDetailsPercentage { get; set; }
        public double? FamilyDetailsPercentage { get; set; }
        public double? UploadDcoumentsPercentage { get; set; }

        public double? AverragePercentage { get; set; }

        public string ErrorMessage { get; set; }
    }
}
