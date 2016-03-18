using System;
using System.Collections;
using System.Collections.Generic;
using Silicus.ProjectTracker.Web.Models;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectStatusAdminDashBoardModel
    {
        IList<ProjectStatusPieChartModel> ProjectStatusPieChartModel { get; set; }

        IList<ProjectTopDefaultersViewModel> ProjectTopDefaultersModel { get; set; }

        IList<ProjectTopSubmittedViewModel> ProjectTopSubmittedModel { get; set; }

        public WeekListData WeekListData { get; set; }
    }
   
}