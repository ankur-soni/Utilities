using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Silicus.ProjectTracker.Services.Interfaces;
using System;
using System.Collections;
using System.Web.UI.WebControls;
using Silicus.ProjectTracker.Core;
using System.Transactions;

namespace Silicus.ProjectTracker.Services
{
    public class ProjectSummaryService : IProjectSummaryService
    {
        private readonly IDataContext context;
        private readonly IProjectResourceService _projectResourceService;
        private readonly IProjectComplaintService _projectComplaintService;
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly IChangeRequestDetailsService _changeRequestDetailsService;
        private readonly IInfrastructureDetailsService _infrastructureDetailsService;
        private readonly IGenericService _genericService;
        private readonly IProjectService _projectService;
        private readonly IProjectMappingService _projectMappingService;

        private bool tabPostedSummary;
        private bool tabPostedSprint;
        private bool tabPostedResource;
        private bool tabPostedComplaint;
        private bool tabPostedPaymentDetail;
        private bool tabPostedChangeRequestDetails;
        private bool tabPostedInfrastructureDetails;

        public ProjectSummaryService(IDataContextFactory dataContextFactory, IProjectResourceService projectResourceService,
            IGenericService genericService, IProjectComplaintService projectComplaintService, IPaymentDetailsService paymentDetailsService,
            IChangeRequestDetailsService changeRequestDetailsService, IInfrastructureDetailsService infrastructureDetailsService,
            IProjectService projectService, IProjectMappingService projectMappingService
            )
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _projectResourceService = projectResourceService;
            _projectComplaintService = projectComplaintService;
            _genericService = genericService;
            _paymentDetailsService = paymentDetailsService;
            _changeRequestDetailsService = changeRequestDetailsService;
            _infrastructureDetailsService = infrastructureDetailsService;
            _projectService = projectService;
            _projectMappingService = projectMappingService;
        }

        public IList<ProjectSummary> GetProjectSprintDetails(int projectId, int WeekId)
        {
            //Fetch the weekid  for week master table
            int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);

            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int currentWeekId = _genericService.GetWeekIdFromMasterTable(currentWeek, DateTime.Now.Year);

            var projectSummary = this.context.Query<ProjectSummary>().Where(p => p.ProjectId == projectId && p.IsActive == true).ToList();

            if (weekId != 0)
            {
                projectSummary = projectSummary.Where(p => p.WeekId == weekId).ToList();
            }

            if (projectSummary.Count() == 0)
            {
                if (weekId >= currentWeekId)
                {
                    ProjectSummary prevWeekData = this.context.Query<ProjectSummary>().Where(p => p.ProjectId == projectId && p.IsActive == true).OrderByDescending(p => p.WeekId).FirstOrDefault();
                    if (prevWeekData != null)
                    {
                        projectSummary = this.context.Query<ProjectSummary>().Where(p => p.ProjectId == projectId && p.IsActive == true && p.WeekId == prevWeekData.WeekId).OrderByDescending(p => p.WeekId).ToList();
                        foreach (var item in projectSummary)
                        {
                            item.ProjectSummaryId = 0;
                        }
                    }
                }

            }

            return projectSummary;
        }

        public int SaveProjectSummary(ProjectStatus ProjectStatus, IList<ProjectSummary> SprintDetails, IList<ProjectResourceUtilization> ProjectResourceDetails,
            IList<ProjectComplaint> ProjectComplaintDetails, IList<PaymentDetails> paymentDetails, IList<ChangeRequestDetails> changeRequestDetails,
            IList<InfrastructureDetails> infrastructureDetails, string userName, string tabsPosted)
        {
            int resultStatus = 0;
            int resultResource = 0;
            int resultComplaint = 0;
            int resultPayment = 0;
            int resultChangeRequest = 0;
            int resultInfrastructure = 0;
            //according to the tabs the data will be posted
            using (TransactionScope scope = new TransactionScope())
            {
                //Fetch the weekid  for week master table
                int weekId = _genericService.GetWeekIdFromMasterTable(ProjectStatus.WeekId, DateTime.Now.Year);

                CheckTabValues(tabsPosted);

                // Status
                if (tabPostedSummary == true)
                {
                    resultStatus = SaveProjectStatus(ProjectStatus, weekId, userName);
                }

                // Sprint
                if (tabPostedSprint == true)
                {
                    SaveSprintDetails(ProjectStatus, SprintDetails, userName, weekId);
                }

                // Resource
                if (tabPostedResource == true)
                {
                    resultResource = _projectResourceService.SaveProjectResources(ProjectResourceDetails, ProjectStatus, weekId, userName);
                }

                //Complaints
                if (tabPostedComplaint == true)
                {
                    resultComplaint = _projectComplaintService.SaveProjectsComplaints(ProjectComplaintDetails, ProjectStatus, userName, weekId);
                }

                if (tabPostedPaymentDetail == true)
                {
                    resultPayment = _paymentDetailsService.SavePaymentDetails(paymentDetails, ProjectStatus, weekId, userName);

                }

                // Change Request
                if (tabPostedChangeRequestDetails == true)
                {
                    resultChangeRequest = _changeRequestDetailsService.SaveChangeRequestDetails(changeRequestDetails, ProjectStatus, weekId, userName);
                }

                // Infrastructure
                if (tabPostedInfrastructureDetails == true)
                {
                    resultInfrastructure = _infrastructureDetailsService.SaveInfrastructureDetails(infrastructureDetails, ProjectStatus, weekId, userName);

                }


                if (resultResource == -1 || resultComplaint == -1 || resultPayment == -1 || resultChangeRequest == -1 || resultInfrastructure == -1 || resultStatus == -1)
                {
                    if (tabPostedSummary == true)
                    {
                        this.context.Delete(ProjectStatus);
                    }
                    if (tabPostedSprint == true)
                    {
                        this.context.DeleteAll(SprintDetails);
                    }
                    if (tabPostedResource == true)
                    {
                        this.context.DeleteAll(ProjectResourceDetails);
                    }
                    if (tabPostedComplaint == true)
                    {
                        this.context.DeleteAll(ProjectComplaintDetails);
                    }
                    if (tabPostedPaymentDetail == true)
                    {
                        this.context.DeleteAll(paymentDetails);
                    }
                    if (tabPostedChangeRequestDetails == true)
                    {
                        this.context.DeleteAll(changeRequestDetails);
                    }
                    if (tabPostedInfrastructureDetails == true)
                    {
                        this.context.DeleteAll(infrastructureDetails);
                    }

                    scope.Dispose();
                    return -1;
                }
                else
                {
                    // Status
                    if (tabPostedSummary == false)
                    {
                        var dbProjectStatus = context.Query<ProjectStatus>()
                                              .Where(ps => ps.WeekId == weekId && ps.ProjectId == ProjectStatus.ProjectId).FirstOrDefault();

                        if (dbProjectStatus != null)
                        {
                            dbProjectStatus.ModifiedBy = userName;
                            dbProjectStatus.ModifiedDate = DateTime.Now;
                            context.Update(dbProjectStatus);
                        }
                    }

                    scope.Complete();
                }

            }

            return 1;
        }

        public void SaveSprintDetails(ProjectStatus ProjectStatus, IList<ProjectSummary> SprintDetails, string userName, int weekId)
        {
            // Save sprint details
            if (SprintDetails != null)
            {
                //Insert Data
                foreach (var lst in SprintDetails)
                {
                    //New record inserted from the grid
                    if (lst.ProjectSummaryId == 0)
                    {
                        lst.CreatedDate = DateTime.Now;
                        lst.CreatedBy = userName;
                        lst.ModifiedBy = userName;
                        lst.ModifiedDate = DateTime.Now;
                        lst.WeekId = weekId;
                        this.context.Add(lst);

                    }
                    //update the existing record 
                    else if (lst.ProjectSummaryId != 0)
                    {
                        lst.ModifiedBy = userName;
                        lst.ModifiedDate = DateTime.Now;
                        this.context.Update(lst);

                    }
                }

                //Delete those sprints which are deleted from frontend
                var lstSummaryProj = context.Query<ProjectSummary>()
                                       .Where(ps => ps.ProjectId == ProjectStatus.ProjectId && ps.WeekId == weekId).ToList();

                foreach (var lstDeleteProj in lstSummaryProj)
                {
                    var isPresent = false;
                    foreach (var lst in SprintDetails)
                    {
                        if (lstDeleteProj.ProjectSummaryId == lst.ProjectSummaryId)
                        {
                            isPresent = true;

                        }

                    }
                    if (isPresent == false)
                    {
                        lstDeleteProj.IsActive = false;
                        this.context.Update(lstDeleteProj);

                    }

                }

            }

        }

        public int SaveProjectStatus(ProjectStatus projectStatus, int weekId, string userName)
        {
            if (projectStatus != null)
            {
                var dbProjectStatus = context.Query<ProjectStatus>()
                    .Where(ps => ps.WeekId == weekId && ps.ProjectId == projectStatus.ProjectId).FirstOrDefault();

                if (dbProjectStatus != null)
                {
                    dbProjectStatus.ModifiedBy = userName;
                    dbProjectStatus.ModifiedDate = DateTime.Now;
                    dbProjectStatus.WeekId = weekId;
                    dbProjectStatus.StatusId = projectStatus.StatusId;
                    dbProjectStatus.ProjectSummary = projectStatus.ProjectSummary;
                    context.Update(dbProjectStatus);

                }
                else
                {
                    projectStatus.CreatedBy = userName;
                    projectStatus.CreatedDate = DateTime.Now;
                    projectStatus.ModifiedBy = userName;
                    projectStatus.ModifiedDate = DateTime.Now;
                    projectStatus.WeekId = weekId;
                    context.Add(projectStatus);

                }

            }
            return projectStatus.ProjectId;
        }

        private void CheckTabValues(string tabsPosted)
        {
            //Splits tab
            var selectedTabs = tabsPosted.Split(',').ToList();
            foreach (var tabWise in selectedTabs)
            {
                var valueTabWise = tabWise.Split(':').ToList();
                string tabName = valueTabWise[0].ToString().TrimStart('{', ' ');
                string tabValue = valueTabWise[1].ToString().TrimEnd(' ', '}');

                if (tabName.Contains("tabPostedSummary"))
                {
                    tabPostedSummary = Convert.ToBoolean(tabValue);
                }
                else if (tabName.Contains("tabPostedSprint"))
                {
                    tabPostedSprint = Convert.ToBoolean(tabValue);
                }
                else if (tabName.Contains("tabPostedResource"))
                {
                    tabPostedResource = Convert.ToBoolean(tabValue);
                }
                else if (tabName.Contains("tabPostedComplaint"))
                {
                    tabPostedComplaint = Convert.ToBoolean(tabValue);
                }
                else if (tabName.Contains("tabPostedPaymentDetails"))
                {
                    tabPostedPaymentDetail = Convert.ToBoolean(tabValue);
                }
                else if (tabName.Contains("tabPostedChangeRequestDetails"))
                {
                    tabPostedChangeRequestDetails = Convert.ToBoolean(tabValue);
                }
                else if (tabName.Contains("tabPostedInfrastructureDetails"))
                {
                    tabPostedInfrastructureDetails = Convert.ToBoolean(tabValue);
                }

            }

        }

        //public int SaveDataFromExcelSheet(IQueryable<LinqToExcel.Row> dataFirstTab, IList<string> worksheetsList, int WeekId, string userName)
        //{
        //    //To check the Excel format in summary tab
        //    bool throwSummaryheaderProperFormat = true;
        //    bool printSummarydata = false;
        //    var icounter = 0;
        //    int icount = 0;
        //    var index = 0;
        //    int iFirstCol = 0;
        //    Project projectDB = new Project();
        //    var project = new Project();
        //    var projectStatus = new ProjectStatus();
        //    int projectId = 0;
        //    var ival = 0;
        //    //Fetch the weekid  for week master table
        //    int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);

        //    //Check Excel format of First tab
        //    foreach (var item in dataFirstTab)
        //    {
        //        icounter = item.Count();

        //        if (printSummarydata == false)
        //        {
        //            if (item[iFirstCol].Value.ToString().TrimEnd().ToLower() == Constants.excelSummaryTabhearderCol1.ToLower())
        //            {
        //                for (icount = 0; icount < icounter - 5; icount++)
        //                {
        //                    switch (icount)
        //                    {
        //                        case 0:
        //                            if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol1.ToLower())
        //                            {
        //                                throwSummaryheaderProperFormat = false;

        //                            }
        //                            break;

        //                        //blank row
        //                        case 1:

        //                            break;

        //                        case 2:
        //                            if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol2.ToLower())
        //                            {
        //                                throwSummaryheaderProperFormat = false;

        //                            }
        //                            break;

        //                        case 3:
        //                            if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol3.ToLower())
        //                            {
        //                                throwSummaryheaderProperFormat = false;

        //                            }
        //                            break;

        //                        case 4:
        //                            if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol4.ToLower())
        //                            {
        //                                throwSummaryheaderProperFormat = false;

        //                            }
        //                            break;

        //                        case 5:
        //                            if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol5.ToLower())
        //                            {
        //                                throwSummaryheaderProperFormat = false;

        //                            }
        //                            break;

        //                        default:
        //                            break;

        //                    }

        //                    if (throwSummaryheaderProperFormat == false)
        //                    {
        //                        return -1;
        //                    }
        //                }

        //                printSummarydata = true;
        //            }
        //        }

        //        else
        //        {
        //            for (icount = 0; icount < icounter - 5; icount++)
        //            {

        //                switch (icount)
        //                {
        //                    case 0:
        //                        if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol1.ToLower())
        //                        {
        //                            throwSummaryheaderProperFormat = false;

        //                        }
        //                        break;

        //                    //blank row
        //                    case 1:

        //                        break;

        //                    case 2:
        //                        if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol2.ToLower())
        //                        {
        //                            throwSummaryheaderProperFormat = false;

        //                        }
        //                        break;

        //                    case 3:
        //                        if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol3.ToLower())
        //                        {
        //                            throwSummaryheaderProperFormat = false;

        //                        }
        //                        break;

        //                    case 4:
        //                        if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol4.ToLower())
        //                        {
        //                            throwSummaryheaderProperFormat = false;

        //                        }
        //                        break;

        //                    case 5:
        //                        if (item[icount].Value.ToString().TrimEnd().ToLower() != Constants.excelSummaryTabhearderCol5.ToLower())
        //                        {
        //                            throwSummaryheaderProperFormat = false;

        //                        }
        //                        break;

        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    return 1;
        //}

        public int SaveDataFromExcelSheet(IList<string> worksheetsList, IQueryable<LinqToExcel.Row> dataFirstTab, IQueryable<LinqToExcel.Row> dataSecondTab, int WeekId, string userName, string destinationPath)
        {
            bool throwErrorVariable = true;
            var icount = 0;
            var index = 0;
            Project projectDB = new Project();
            var project = new Project();
            var projectStatus = new ProjectStatus();
            int projectId = 0;

            //Fetch the weekid  for week master table
            int weekId = _genericService.GetWeekIdFromMasterTable(WeekId, DateTime.Now.Year);

            foreach (var item in dataFirstTab)
            {
                icount = item.Count() - 5;
                if (index >= 5)
                {
                    var ival = 0;
                    string firstCellValue = item[ival].Value.ToString();

                    if (string.IsNullOrEmpty(firstCellValue))
                    {
                        break;
                    }
                    string[] arrayData = new string[6];

                    if (string.IsNullOrEmpty(item[ival].Value.ToString()))
                    {
                        throwErrorVariable = false;
                        break;
                    }
                    else
                    {
                        string name = item[ival].Value.ToString();
                        if (worksheetsList.Contains(name))
                        {
                            project.ProjectName = name;
                        }
                        else
                        {
                            throwErrorVariable = false;
                            break;
                        }
                    }

                    //Get the projectid from project table
                    projectDB = _projectService.GetProjectByProjectName(project.ProjectName);
                    project.ProjectId = projectDB.ProjectId;
                    projectStatus.ProjectStatusId = 0;
                    projectStatus.ProjectId = projectDB.ProjectId;
                    projectId = projectDB.ProjectId;

                    ival = ival + 2;
                    if (string.IsNullOrEmpty(item[ival].Value.ToString()))
                    {
                        throwErrorVariable = false;
                        break;
                    }
                    else
                    {
                        DateTime startDate = Convert.ToDateTime(item[ival].Value);
                        project.StartDate = startDate;
                    }

                    ival++;

                    if (string.IsNullOrEmpty(item[ival].Value.ToString()))
                    {
                        throwErrorVariable = false;
                        break;
                    }
                    else
                    {
                        project.PlannedEndDate = Convert.ToDateTime(item[ival].Value);
                    }
                    ival++;

                    string statusName = item[ival].Value.ToString();
                    int statusId = this.context.Query<Status>().Where(p => p.StatusName.ToUpper() == statusName.TrimEnd().ToUpper()).Select(p => p.StatusId).FirstOrDefault();
                    projectStatus.StatusId = statusId;

                    ival++;
                    if (string.IsNullOrEmpty(item[ival].Value.ToString()))
                    {
                        throwErrorVariable = false;
                        break;
                    }
                    else
                    {
                        projectStatus.ProjectSummary = item[ival].Value.ToString();
                        project.ProjectDescription = item[ival].Value.ToString();
                    }

                    project.CreatedDate = DateTime.Now;
                    project.CreatedBy = userName;
                    project.ModifiedDate = DateTime.Now;
                    project.ModifiedBy = userName;


                    projectStatus.CreatedBy = userName;
                    projectStatus.CreatedDate = DateTime.Now;
                    projectStatus.ModifiedBy = userName;
                    projectStatus.ModifiedDate = DateTime.Now;

                    this.SaveProjectStatus(projectStatus, weekId, userName);

                }

                index++;
            }

            if (throwErrorVariable == false)
            {
                return -1;
            }

            index = 0;
            var ivalItem = 0;
            string sprintId = string.Empty;
            string mileStoneId = string.Empty;
            IList<ProjectSummary> prjSprintModelJs = new List<ProjectSummary>();

            foreach (var secondTab in dataSecondTab)
            {
                if (index >= 7)
                {
                    var ival = 0;
                    string firstCellValue = secondTab[ival].Value.ToString();

                    if (string.IsNullOrEmpty(firstCellValue))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(secondTab[ival].Value.ToString()))
                    {
                        throwErrorVariable = false;
                        break;
                    }
                    else
                    {
                        string name = secondTab[ival].Value.ToString();
                        if (worksheetsList.Contains(name))
                        {
                            project.ProjectName = name;
                        }
                        else
                        {
                            throwErrorVariable = false;
                            break;
                        }
                    }
                }

                //Get the sprint details from summary table
                var projectSummaryDB = GetProjectSprintDetailsByProjectName(project.ProjectName);


                foreach (var item in worksheetsList)
                {
                    //string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Upload/File.xlsx");

                    var bookFirst = new LinqToExcel.ExcelQueryFactory(destinationPath);
                    var nextTab = from x in bookFirst.Worksheet(item.TrimEnd())
                                  select x;
                    int resourceStartRowNumber = 0;

                    foreach (var itemValue in nextTab)
                    {
                        if (itemValue[ivalItem].Value.ToString().ToLower() == Constants.excelResourceTableHeaderMain.ToLower())
                        {
                            resourceStartRowNumber = index;
                        }

                        if (index == 3)
                        {
                            ivalItem = 1;
                            //get sprint and milestone id
                            sprintId = itemValue[ivalItem].Value.ToString();

                            ivalItem = 4;
                            mileStoneId = itemValue[ivalItem].Value.ToString();

                        }
                        
                        if (index >= 7)
                        {
                            if ((itemValue[ivalItem].Value == DBNull.Value) || (itemValue[ivalItem].Value.ToString().TrimEnd().ToLower() == Constants.excelResourceTableHeaderMain.ToLower()))
                            {
                                break; 
                            }

                            ProjectSummary prjSprintSingleRecord = new ProjectSummary();
                            prjSprintSingleRecord.ProjectSummaryId = 0;
                            prjSprintSingleRecord.ProjectId = projectId;
                            prjSprintSingleRecord.SprintId = Convert.ToInt16(sprintId);
                            prjSprintSingleRecord.MileStoneId = Convert.ToInt16(mileStoneId);
                            ivalItem = 1;
                            prjSprintSingleRecord.StartDate = Convert.ToDateTime(itemValue[ivalItem].Value);
                            ivalItem++;
                            prjSprintSingleRecord.EndDate = Convert.ToDateTime(itemValue[ivalItem].Value);
                            ivalItem = ivalItem + 2;
                            prjSprintSingleRecord.ReleaseDate = Convert.ToDateTime(itemValue[ivalItem].Value);
                            ivalItem++;
                            prjSprintSingleRecord.Remarks = itemValue[ivalItem].Value.ToString();
                            prjSprintSingleRecord.CreatedBy = userName;
                            prjSprintSingleRecord.CreatedDate = DateTime.Now;
                            prjSprintSingleRecord.ModifiedBy = userName;
                            prjSprintSingleRecord.ModifiedDate = DateTime.Now;
                            prjSprintModelJs.Add(prjSprintSingleRecord);

                            foreach (var lst in prjSprintModelJs)
                            {
                                //New record inserted from the grid
                                if (lst.ProjectSummaryId == 0)
                                {
                                    lst.CreatedDate = DateTime.Now;
                                    lst.CreatedBy = userName;
                                    lst.ModifiedBy = userName;
                                    lst.ModifiedDate = DateTime.Now;
                                    lst.WeekId = weekId;
                                    this.context.Add(lst);
                                }
                            }

                            //SaveSprintDetails(projectStatus, prjSprintModelJs, userName, weekId);
                        }

                        index++;
                    }
                }
            }

            var ivalResourceItem = 0;
            var index1 = 0;

            foreach (var item1 in worksheetsList)
            {
                var bookFirst1 = new LinqToExcel.ExcelQueryFactory(destinationPath);
                var nextTab1 = from x in bookFirst1.Worksheet(item1.TrimEnd())
                              select x;

                foreach (var itemResourceValue in nextTab1)
                {
                    //ivalResourceItem = 1;
                    if (itemResourceValue[ivalResourceItem].Value.ToString().TrimEnd().ToLower() == Constants.excelResourceTableHeaderCol1.ToLower())
                    {
                        // work in progress......

                        index1++;

                    }

                    index1++;
                }
            }
            
            return 1;
            
        }

        private IList<ProjectSummary> GetProjectSprintDetailsByProjectName(string projectName)
        {
            var project = context.Query<Project>().Where(p => p.ProjectName == projectName).FirstOrDefault();
            var projectSummary = context.Query<ProjectSummary>().Where(ps => ps.ProjectId == project.ProjectId);

            //var projectSummary = context.Query<ProjectSummary>()
            //    .Join(context.Query<Project>().Where(pp => pp.ProjectName == projectName), ps => ps.ProjectId, pr => pr.ProjectId, (ps, pr) => new { ps });

            return projectSummary.ToList();
        }
    }
}
