using System.Linq;
using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IProjectSummaryService
    {
        IList<ProjectSummary> GetProjectSprintDetails(int projectId, int weekId);

        int SaveProjectSummary(ProjectStatus ProjectStatus, IList<ProjectSummary> SprintDetails, IList<ProjectResourceUtilization> ProjectResourceDetails,
            IList<ProjectComplaint> ProjectComplaintDetails, IList<PaymentDetails> paymentDetails, IList<ChangeRequestDetails> changeRequestDetails,
            IList<InfrastructureDetails> infrastructureDetails, string userName, string tabsPosted);

        int SaveProjectStatus(ProjectStatus projectStatus, int weekId, string userName);

        int SaveDataFromExcelSheet(IList<string> worksheetsList, IQueryable<LinqToExcel.Row> excelFirstTabData, IQueryable<LinqToExcel.Row> excelSecondTabData, int WeekId, string userName, string destinationPath);
    }
}
