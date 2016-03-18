using System;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Web.ViewModel
{  
    public class ProjectMainViewModel
    {
        public ProjectViewModel ProjectViewModel { get; set; }

        public IList<ProjectSummaryViewModel> ProjectSummaryViewModel { get; set; }

        public IList<ProjectResourceUtilizationViewModel> ProjectResouceUtilizationViewModel { get; set; }

        public IList<ProjectComplaintViewModel> ProjectComplaintViewModel { get; set; }

        public IList<PaymentDetailsViewModel> PaymentDetailsViewModel { get; set; }

        public IList<InfrastructureDetailsViewModel> InfrastructureDetailsViewModel { get; set; }

        public IList<ChangeRequestDetailsViewModel> ChangeRequestDetailsViewModel { get; set; }

        public string SelectedWeek { get; set; }

        public int CurrentWeek { get; set; }

        public IEnumerable<SelectListItem> WeekList { get; set; }

        public string IsSuccess { get; set; }
        
    }
}
