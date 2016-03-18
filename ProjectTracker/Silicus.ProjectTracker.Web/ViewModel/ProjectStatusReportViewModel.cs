using System.Collections.Generic;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ProjectStatusReportViewModel
    {
        public ProjectStatusReportViewModel()
        {
            TotalWeeksToDisplay = 5;
        }

        public ProjectData ProjectData { get; set; }

        public int CurrentWeek { get; set; }

        public int TotalWeeksToDisplay { get; set; }

        public IList<WeekListData> WeekListData { get; set; }
    }

    public class WeekData
    {
        public int WeekId { get; set; }

        public int StatusId { get; set; }
    }

    public class ProjectData
    {
        public string ProjectName { get; set; }

        public int ProjectId { get; set; }

        public IList<WeekData> WeekData { get; set; }        
    }

    public class WeekListData
    {
        public int WeekId { get; set; }

        public int Year { get; set; }

        public int WeekNumber { get; set; }

        public string Text { get; set; }
    }
}