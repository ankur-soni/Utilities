using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class UserReminder
    {
        public Nullable<long> UserId { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public System.DateTime LastLogin { get; set; }
        public Nullable<int> NumberOfReminders { get; set; }
    }
}
