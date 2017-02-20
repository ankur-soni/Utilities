using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.Ensure.Web.Models
{
    public class RecruiterViewModel
    {
        public int RecruiterId { get; set; }

        public string RecruiterName { get; set; }

        public bool IsAssignedRecruiter { get; set; }
    }
}