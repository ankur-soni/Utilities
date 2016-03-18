using System;
using System.Linq;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class GenericService:IGenericService
    {
        private readonly IDataContext context;

        public GenericService(IDataContextFactory dataContextFactory)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public int GetWeekIdFromMasterTable(int weekNumber, int year)
        {
            Week weekId = this.context.Query<Week>().Where(w => w.Year == year && w.WeekNumber == weekNumber).FirstOrDefault();
            return weekId.WeekId;
        }

        public IList<WeekModel> GetWeeksOfTheYear(int year)
        {
            var weekListFromDb = context.Query<Week>().Where(w => w.Year == year).ToList();
            var weeksOfTheYear =
                weekListFromDb.Select(z => new WeekModel
                {
                    Number = z.WeekNumber,
                    Text = z.Text,
                    BeginningOfWeek = GetFirstDayOfWeek(z.WeekNumber, year)
                });

            return weeksOfTheYear.ToList();
        }

        private DateTime GetFirstDayOfWeek(int weekNumber, int year)
        {
            var firstDayOfYear = new System.DateTime(year, 1, 1);
            var beginningDayOfWeek = firstDayOfYear.AddDays(-1 * Convert.ToInt32(firstDayOfYear.DayOfWeek));
            return beginningDayOfWeek.AddDays(7 * (weekNumber));
        }

        public int GetWeek(DateTime date)
        {
            System.Globalization.CultureInfo cult_info = System.Globalization.CultureInfo.CreateSpecificCulture("no");
            System.Globalization.Calendar cal = cult_info.Calendar;
            int weekCount = cal.GetWeekOfYear(date, cult_info.DateTimeFormat.CalendarWeekRule, cult_info.DateTimeFormat.FirstDayOfWeek);
            return weekCount;
        }

        public IList<Sprints> GetSprintCounts()
        {
            List<Sprints> sprintCountsList = new List<Sprints>();
            int sprintCounts = Convert.ToInt16(
                    WebConfigurationManager.AppSettings["SprintCounts"]);

            //Default value
            if (sprintCounts.ToString() == null || sprintCounts != 0)
            {
                sprintCounts = Constants.DefaultSprintCounts;

            }

            Sprints sprintDefault = new Sprints();
            sprintDefault.Value = -1;
            sprintDefault.Text = "-Select-";
            sprintCountsList.Add(sprintDefault);

            for (int iSprintCount = 1; iSprintCount <= sprintCounts; iSprintCount++)
            {
                Sprints sprintSingleCount = new Sprints();
                sprintSingleCount.Value = iSprintCount;
                sprintSingleCount.Text = "Sprint" + iSprintCount;
                sprintCountsList.Add(sprintSingleCount);

            }

            return sprintCountsList;
        }

        public string GetSprintName(int sprintId)
        {
            return "Sprint" + sprintId;
        }

        public string GetMilestoneName(int milestoneId)
        {
            return "Milestone" + milestoneId;
        }

        public IList<Sprints> GetMileStoneCounts()
        {
            List<Sprints> milestoneCountsList = new List<Sprints>();
            int milestoneCounts = Convert.ToInt16(
                    WebConfigurationManager.AppSettings["MileStoneCounts"]);

            //Default value
            if (milestoneCounts.ToString() == null || milestoneCounts != 0)
            {
                milestoneCounts = Constants.DefaultMilestoneCounts;

            }

            Sprints sprintDefault = new Sprints();
            sprintDefault.Value = -1;
            sprintDefault.Text = "-Milestone-";
            milestoneCountsList.Add(sprintDefault);

            for (int iSprintCount = 1; iSprintCount <= milestoneCounts; iSprintCount++)
            {
                Sprints milestoneSingleCount = new Sprints();
                milestoneSingleCount.Value = iSprintCount;
                milestoneSingleCount.Text = "MileStone" + iSprintCount;
                milestoneCountsList.Add(milestoneSingleCount);

            }

            return milestoneCountsList;
        }
        
        public List<WeekYear> GetWeekNumbers(DateTime startDate, DateTime endDate)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            List<WeekYear> list = new List<WeekYear>();

            for (DateTime tm = startDate; tm <= endDate; tm = tm.AddDays(7))
            {
                WeekYear wy;
                wy.weekId = currentCulture.Calendar.GetWeekOfYear(tm, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                wy.year = currentCulture.Calendar.GetYear(tm);
                list.Add(wy);
            }

            return list;
        }

        public Week GetWeekNumberYearByWeekId(int WeekId)
        {
            var weekInfo = context.Query<Week>().FirstOrDefault(w => w.WeekId == WeekId);

            return weekInfo;
        }

        public IList<WeekModel> PrepareWeekList(int currentWeek)
        {
            var getWeekList = new List<WeekModel>();
            var getWeekListNew = new List<WeekModel>();
            var getWeekListFinal = new List<WeekModel>();

            DateTime startDate = DateTime.Now.AddMonths(-Convert.ToInt16(ConfigurationManager.AppSettings["PastWeekEntryInMonths"]));
            getWeekList = GetWeeksOfTheYear(startDate.Year).ToList();

            //if start date is not current year
            if (startDate.Year != DateTime.Now.Year)
            {
                int weekNumber = GetWeek(startDate);
                getWeekList = getWeekList.Where(p => p.Number >= weekNumber).OrderByDescending(p => p.Number).ToList();

                getWeekListNew = GetWeeksOfTheYear(DateTime.Now.Year).ToList();
                int toWeekKey = Convert.ToInt16(ConfigurationManager.AppSettings["FutureWeekEntryInWeeks"]);
                int calculatedWeek = currentWeek + toWeekKey;
                getWeekListNew = getWeekListNew.Where(p => p.Number <= calculatedWeek).OrderByDescending(p => p.Number).ToList();

                foreach (var item in getWeekListNew)
                {
                    getWeekListFinal.Add(item);

                }

                foreach (var item in getWeekList)
                {
                    var isExists = getWeekListFinal.Find(p => p.BeginningOfWeek == item.BeginningOfWeek);
                    if (isExists == null)
                    {
                        getWeekListFinal.Add(item);
                    }
                }
            }
            else
            {
                int toWeekKey = Convert.ToInt16(ConfigurationManager.AppSettings["FutureWeekEntryInWeeks"]);
                int calculatedWeek = currentWeek + toWeekKey;
                getWeekListFinal = getWeekList.Where(p => p.Number <= calculatedWeek).OrderByDescending(p => p.Number).ToList();
            }

            return getWeekListFinal;
        }               
    }
}