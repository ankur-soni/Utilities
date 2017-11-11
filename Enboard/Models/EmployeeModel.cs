using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class EmployeeModel
    {
        public long Id { get; set; }
        public string EmpNo { get; set; }
        public string EmployeeName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<int> Active { get; set; }
        public string ReasonForleaving { get; set; }
        public long UserId { get; set; }
        public Nullable<System.DateTime> LeavingDate { get; set; }
    }
}
