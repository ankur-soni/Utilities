using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectTopDefaultersModel
    {
        public int projectId { get; set; }

        public DateTime projectStartDate { get; set; }

        public string projectName { get; set; }

        public string userName { get; set; }

        public string weeks { get; set; }
    }
}
