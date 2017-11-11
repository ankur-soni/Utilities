using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ActivityDetails
    {
        public Nullable<System.DateTime> PersonalDetailsDate { get; set; }
        public Nullable<System.DateTime> ContactDetailsDate { get; set; }
        public Nullable<System.DateTime> EducationDetailsDate { get; set; }
        public Nullable<System.DateTime> EmploymentDetailsDate { get; set; }
        public Nullable<System.DateTime> FamilyDetailsDate { get; set; }
        public Nullable<System.DateTime> UploadDocumentDetailsDate { get; set; }
    }
}
