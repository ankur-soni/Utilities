using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectTopSubmittedModel
    {
        public int projectId { get; set; }
                
        public string projectName { get; set; }

        public string userName { get; set; }

        public string status { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime submittedDate { get; set; }
    }
}
