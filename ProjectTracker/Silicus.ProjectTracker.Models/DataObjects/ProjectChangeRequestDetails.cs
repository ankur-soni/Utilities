using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectChangeRequestDetails
    {
        public int ChangeRequestId { get; set; }

        public int ChangeRequestNumber { get; set; }

        public DateTime RecievedDate { get; set; }

        public string Status { get; set; }
        //Don't delete this field
        public string userName { get; set; }
    }
}
