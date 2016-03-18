using System.Collections.Generic;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Web.Models;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        IList<ProjectStatusPieChartModel> GetProjectStatusDataForPieChart();

        IList<ProjectTopDefaultersModel> GetDataForDefaulterList();

        IList<ProjectTopSubmittedModel> GetListForStatusReportSubmitted();

    }
}
