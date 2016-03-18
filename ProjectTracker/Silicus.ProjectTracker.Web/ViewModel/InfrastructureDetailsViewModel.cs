using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class InfrastructureDetailsViewModel : BaseEntity
    {
        public int InfrastructureDetailId { get; set; }

        public string DevelopmentAndQA { get; set; }

        public string UAT { get; set; }

        public string Production { get; set; }

        public int ProjectId { get; set; }
        
        public int WeekId { get; set; }
    }
}