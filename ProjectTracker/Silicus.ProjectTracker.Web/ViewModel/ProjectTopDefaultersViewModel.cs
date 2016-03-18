using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectTopDefaultersViewModel
    {
        [ScaffoldColumn(false)]
        public int defaulterId { get; set; }

        public string projectName { get; set; }

        public string userName { get; set; }

        public int weeks { get; set; }
    }
}