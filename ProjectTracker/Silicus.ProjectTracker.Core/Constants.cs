using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Silicus.ProjectTracker.Core
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public static readonly string SuccessMessage = "SUCCESS";
        public static readonly string FailureMessage = "FAILURE";
        public static readonly string UserGroup = "ProjectTrackerUser";
        public static readonly string AdminGroup = "ProjectTrackerAdmin";
        public static readonly string UserRole = "User";
        public static readonly string AdminRole = "Admin";
        public static readonly string DomainName = "Silicus";
        public static readonly int DefaultSprintCounts = 10;
        public static readonly int DefaultMilestoneCounts = 10;

        public static readonly string excelSummaryTabhearderCol1 = "Project Name";
        public static readonly string excelSummaryTabhearderCol2 = "Start Date";
        public static readonly string excelSummaryTabhearderCol3 = "Planned End Date";
        public static readonly string excelSummaryTabhearderCol4 = "Status";
        public static readonly string excelSummaryTabhearderCol5 = "Summary of Project Status";

        public static readonly string excelResourceTableHeaderMain = "Resource & Effort Details";
        public static readonly string excelResourceTableHeaderCol1 = "Role(As per SOW)";

     }
}
