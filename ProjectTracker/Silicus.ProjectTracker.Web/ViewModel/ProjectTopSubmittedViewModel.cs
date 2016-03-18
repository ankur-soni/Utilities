using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectTopSubmittedViewModel
    {
        [ScaffoldColumn(false)]
        public int defaulterId { get; set; }

        public string projectName { get; set; }

        public string userName { get; set; }

        public string status { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime submittedDate { get; set; }
    }
}