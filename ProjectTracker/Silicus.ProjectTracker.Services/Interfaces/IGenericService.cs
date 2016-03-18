using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using System;

namespace Silicus.ProjectTracker.Services.Interfaces
{
    public interface IGenericService
    {
        int GetWeekIdFromMasterTable(int weekNumber, int year);

        IList<WeekModel> GetWeeksOfTheYear(int year);

        int GetWeek(DateTime dateTime);

        IList<Sprints> GetSprintCounts();

        IList<Sprints> GetMileStoneCounts();

        string GetSprintName(int sprintid);

        string GetMilestoneName(int milestoneId);

        //Dictionary<int, int> GetWeekNumbers(DateTime startDate, DateTime endDate);

        List<WeekYear> GetWeekNumbers(DateTime startDate, DateTime endDate);
        
        Week GetWeekNumberYearByWeekId(int WeekId);

        IList<WeekModel> PrepareWeekList(int currentWeek);
    }
}
