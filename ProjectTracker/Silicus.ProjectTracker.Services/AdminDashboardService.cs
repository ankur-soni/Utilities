using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Web.Models;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IDataContext context;
        private readonly IGenericService _genericService;

        public AdminDashboardService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;

        }

        public IList<ProjectStatusPieChartModel> GetProjectStatusDataForPieChart()
        {
            IList<ProjectStatus> projects = new List<ProjectStatus>();
            IEnumerable<ProjectStatusPieChartModel> query = new List<ProjectStatusPieChartModel>();
            int weekId = GetExistingWeekIdData();
            if (weekId != 0)
            {
                projects = this.context.Query<ProjectStatus>().Where(pt => pt.WeekId == weekId).ToList();
            }

            if (projects.Count() != 0)
            {
                IList<Status> status = this.context.Query<Status>().OrderBy(s => s.StatusName).ToList();
                query = status.Select(
                    ep => new ProjectStatusPieChartModel
                    {
                        project = ep.StatusName,
                        percentage = projects.Count(epc => epc.StatusId == ep.StatusId)
                    });
            }

            return query.ToList();
        }
        
        public IList<ProjectTopDefaultersModel> GetDataForDefaulterList()
        {
            int weekId = GetExistingWeekIdData();
            List<ProjectMapping> projectMapping = this.context.Query<ProjectMapping>().Where(p => p.IsDeleted == false).GroupBy(p => p.ProjectId).Select(y => y.FirstOrDefault()).ToList();
            List<Project> project = this.context.Query<Project>().Where(p => p.IsActive == true).ToList();
            List<ProjectStatus> projectStatus = this.context.Query<ProjectStatus>().Where(ps => ps.WeekId == weekId).ToList();
            var query =
                    from u in project
                    from p in projectStatus.Where(p => p.ProjectId == u.ProjectId).DefaultIfEmpty()
                    from o in projectMapping.Where(o => o.ProjectId == u.ProjectId)
                    select new ProjectTopDefaultersModel
                    {
                        projectId = u.ProjectId,
                        projectStartDate = u.StartDate,
                        projectName = u.ProjectName,
                        userName = o.UserName,
                        weeks = ""
                    };


            IList<ProjectTopDefaultersModel> finalList = new List<ProjectTopDefaultersModel>();
            foreach (var qdata in query)
            {
                ProjectTopDefaultersModel singleRecord = new ProjectTopDefaultersModel();
                singleRecord.projectName = qdata.projectName;
                singleRecord.userName = qdata.userName;
                DateTime currentDate = DateTime.Now;
                List<WeekYear> result = new List<WeekYear>();
                result = this._genericService.GetWeekNumbers(qdata.projectStartDate, DateTime.Now);
                int defaulterCount = 0;

                foreach (var wy in result)
                {
                    var weekIdFromMasterTable = _genericService.GetWeekIdFromMasterTable(wy.weekId, wy.year);
                    var isPresent = this.context.Query<ProjectStatus>().Where(p => p.ProjectId == qdata.projectId).Any(q => q.WeekId == weekIdFromMasterTable);
                    if (Convert.ToInt32(isPresent) == 0)
                    {
                        defaulterCount++;
                    }
                }

                qdata.weeks = (defaulterCount == 0 ? "0" : Convert.ToString(defaulterCount));
                singleRecord.weeks = qdata.weeks;
                finalList.Add(singleRecord);

            }


            return finalList;
        }

        public IList<ProjectTopSubmittedModel> GetListForStatusReportSubmitted()
        {
            IList<ProjectTopSubmittedModel> query = new List<ProjectTopSubmittedModel>();
            int weekId = GetExistingWeekIdData();
            List<ProjectMapping> projectMapping = this.context.Query<ProjectMapping>().Where(p => p.IsDeleted == false).GroupBy(p => p.ProjectId).Select(y => y.FirstOrDefault()).ToList();
            IList<Project> project = this.context.Query<Project>().Where(p => p.IsActive == true).ToList();
            IList<ProjectStatus> projectStatus = this.context.Query<ProjectStatus>().Where(ps => ps.WeekId == weekId).OrderByDescending(p => p.ModifiedDate).ToList();
            IList<Status> status = this.context.Query<Status>().ToList();
            if (projectStatus.Count() != 0)

            {
                query =
                      (from u in projectStatus
                       from p in project.Where(p => p.ProjectId == u.ProjectId).DefaultIfEmpty()
                       from o in projectMapping.Where(o => o.ProjectId == u.ProjectId)
                       from s in status.Where(s => s.StatusId == u.StatusId)
                       select new ProjectTopSubmittedModel
                       {
                           projectId = u.ProjectId,
                           userName = o.UserName,
                           status = s.StatusName,
                           submittedDate = p != null ? u.ModifiedDate : DateTime.MinValue,
                           projectName = p != null ? p.ProjectName : "",

                       }).ToList();

             }

            return query.ToList();
                       

        }

        private int GetExistingWeekIdData()
        {
            var previousWeekId = 0;
            int currentWeekId = this._genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int weekId = _genericService.GetWeekIdFromMasterTable(currentWeekId, DateTime.Now.Year);
            IList<ProjectStatus> projects = this.context.Query<ProjectStatus>().Where(pt => pt.WeekId == weekId).ToList();
            if (projects.Count() == 0)
            {
                var prevWeekId = this.context.Query<ProjectStatus>().OrderByDescending(p => p.WeekId).FirstOrDefault();
                if (prevWeekId == null)
                {
                    previousWeekId = 0;
                }
                else
                {
                    previousWeekId = prevWeekId.WeekId;
                }
            }
            else
            {
                previousWeekId = weekId;
            }
            return previousWeekId;

        }
    }
}
