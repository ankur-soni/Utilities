using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectInfrastructureDetails
    {
        public int InfrastructureId { get; set; }

        public string DevQA { get; set; }

        public string UAT { get; set; }

        public string Production { get; set; }
         //Don't delete this field
        public string userName { get; set; }
    }
}
