using System;
using System.Web;
using System.Linq;
using Kendo.Mvc.UI;
using System.Web.Mvc;
using System.Configuration;
using Kendo.Mvc.Extensions;
using System.Collections;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Web;
using Silicus.ProjectTracker.Web.Hubs;
using Silicus.ProjectTracker.Web.Filters;
using Silicus.ProjectTracker.Web.Models;
using Silicus.ProjectTracker.Web.ViewModel;
using Silicus.ProjectTracker.Core.Interfaces;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;
using System.IO;

namespace Silicus.ProjectTracker.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IProjectSummaryService _projectSummaryService;
        private readonly IProjectComplaintService _projectComplaintService;
        private readonly IMappingService _mappingService;
        private readonly ICookieHelper _cookieHelper;
        private readonly IProjectResourceService _projectResourceService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IGenericService _genericService;
        private readonly IPaymentDetailsService _paymentDetailsService;
        private readonly IChangeRequestDetailsService _changeRequestDetailsService;
        private readonly IInfrastructureDetailsService _infrastructureDetailsService;
        private readonly IUserDashboardService _userDashboardService;
        private readonly ITrackerHub _itrackerHub;
      
        public UserController(IProjectService projectService, IMappingService mappingService, IProjectSummaryService projectSummaryService,
            ICookieHelper cookieHelper, IProjectResourceService projectResourceService, IActiveDirectoryService activeDirectoryService,
            IProjectComplaintService projectComplaintService, IGenericService genericService, IPaymentDetailsService paymentDetailsService,
            IChangeRequestDetailsService changeRequestDetailsService, IInfrastructureDetailsService infrastructureDetailsService,
            IUserDashboardService userDashboardService, ITrackerHub itrackerHub
            )
        {
            this._projectService = projectService;
            this._mappingService = mappingService;
            this._projectSummaryService = projectSummaryService;
            this._cookieHelper = cookieHelper;
            this._projectResourceService = projectResourceService;
            this._activeDirectoryService = activeDirectoryService;
            this._projectComplaintService = projectComplaintService;
            this._genericService = genericService;
            this._paymentDetailsService = paymentDetailsService;
            this._changeRequestDetailsService = changeRequestDetailsService;
            this._infrastructureDetailsService = infrastructureDetailsService;
            this._userDashboardService = userDashboardService;
            this._itrackerHub = itrackerHub;
       }

        [AuthorizeADAttribute(Groups = "ProjectTrackerUser, ProjectTrackerAdmin")]
        public ActionResult Dashboard()
        {
            var projectStatusReportViewModelList = LoadDashboardData();
                        
            return View(projectStatusReportViewModelList);
        }

        public ActionResult ProjectDetailById(int projectId)
        {
            //Main model
            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            //Fill all sub models
            projectMainViewModel.ProjectViewModel = GetProjectSummaryDetails(projectId);
            projectMainViewModel.ProjectResouceUtilizationViewModel = new List<ProjectResourceUtilizationViewModel>();
            projectMainViewModel.ProjectSummaryViewModel = new List<ProjectSummaryViewModel>();
            projectMainViewModel.ProjectComplaintViewModel = new List<ProjectComplaintViewModel>();

            return PartialView("_ProjectDetails", projectMainViewModel);
        }

        public ActionResult ProjectDetailByIdAndWeek(int projectId, int weekNumber)
        {
            //Main model
            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            //Fill all sub models
            projectMainViewModel.ProjectViewModel = GetProjectSummaryDetails(projectId);
            projectMainViewModel.ProjectResouceUtilizationViewModel = new List<ProjectResourceUtilizationViewModel>();
            projectMainViewModel.ProjectSummaryViewModel = new List<ProjectSummaryViewModel>();
            projectMainViewModel.ProjectComplaintViewModel = new List<ProjectComplaintViewModel>();

            var week = _genericService.GetWeekNumberYearByWeekId(weekNumber);

            projectMainViewModel.CurrentWeek = week.WeekNumber;

            return PartialView("_ProjectDetails", projectMainViewModel);
        }

        public ActionResult GetProjectTypeDetails([DataSourceRequest] DataSourceRequest request, int projectId, int WeekId)
        {
            var projectMainViewModel = new ProjectMainViewModel();
            var prjSummaryModel = _projectSummaryService.GetProjectSprintDetails(projectId, WeekId).ToList();
            var prjSummaryViewModels = _mappingService.Map<IList<ProjectSummary>, IList<ProjectSummaryViewModel>>(prjSummaryModel);
            projectMainViewModel.ProjectSummaryViewModel = prjSummaryViewModels;

            foreach (var projectSummaryViewModel in projectMainViewModel.ProjectSummaryViewModel)
            {
                if (projectSummaryViewModel.SprintId == 0)
                {
                    projectSummaryViewModel.SprintName = string.Empty;
                }
                else
                {
                    projectSummaryViewModel.SprintName = _genericService.GetSprintName(projectSummaryViewModel.SprintId);
                }
                projectSummaryViewModel.Sprints = _genericService.GetSprintCounts().ToList();


                if (projectSummaryViewModel.MileStoneId == 0)
                {
                    projectSummaryViewModel.MilestoneName = string.Empty;
                }
                else
                {
                    projectSummaryViewModel.MilestoneName = _genericService.GetMilestoneName(projectSummaryViewModel.MileStoneId);
                }
                projectSummaryViewModel.Milestones = _genericService.GetMileStoneCounts().ToList();

            }

            return Json(projectMainViewModel.ProjectSummaryViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectsByUsername([DataSourceRequest] DataSourceRequest request, string username = "")
        {
            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            ViewBag.AdminMenuVisibilty = false;

            string userName = _cookieHelper.GetCookie("userid");

            if (username != "")
            {
                userName = username;
            }

            var projects = this._projectService.GetProjectsByUsername(userName).ToList();

            var projectViewModels = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                var projectSummary = GetProjectSummaryDetails(project.ProjectId);
                projectSummary.UserName = username;
                projectViewModels.Add(projectSummary);
            }

            return Json(projectViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAjaxTab(int tabId, int ProjectId = 0, int Weekid = 0)
        {
            string viewName = string.Empty;

            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            switch (tabId)
            {
                case 1:
                    viewName = "_Summary";
                    return PartialView(viewName, GetProjectSummaryDetails(ProjectId, Weekid));
                case 2:
                    viewName = "_SprintDetails";
                    return PartialView(viewName, projectMainViewModel.ProjectSummaryViewModel);
                case 3:
                    viewName = "_ResourceDetails";
                    return PartialView(viewName, projectMainViewModel.ProjectResouceUtilizationViewModel);
                case 4:
                    viewName = "_Complaints";
                    return PartialView(viewName, projectMainViewModel.ProjectComplaintViewModel);
                case 5:
                    viewName = "_PaymentDetails";
                    return PartialView(viewName, projectMainViewModel.PaymentDetailsViewModel);
                case 6:
                    viewName = "_ChangeRequestDetails";
                    return PartialView(viewName, projectMainViewModel.ChangeRequestDetailsViewModel);
                case 7:
                    viewName = "_InfrastructureDetails";
                    return PartialView(viewName, projectMainViewModel.InfrastructureDetailsViewModel);
                case 8:
                    viewName = "_error";
                    return PartialView(viewName, projectMainViewModel.ProjectViewModel);
            }

            return PartialView(viewName, projectMainViewModel.ProjectViewModel);

        }

        public ActionResult GetSprints(List<string> values)
        {
            var getSprintCountList = _genericService.GetSprintCounts();
            return Json(getSprintCountList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMilestones(List<string> values)
        {
            var getSprintCountList = _genericService.GetMileStoneCounts();
            return Json(getSprintCountList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProjectSummary(ProjectStatus ProjectStatus, IList<ProjectSummaryViewModel> SprintDetails, IList<ProjectResourceUtilizationViewModel> ResourceDetails,
            IList<ProjectComplaintViewModel> ComplaintDetails, IList<PaymentDetailsViewModel> PaymentDetails, IList<ChangeRequestDetailsViewModel> ChangeRequestDetails,
            IList<InfrastructureDetailsViewModel> InfrastructureDetails, string tabsPosted)
        {
            string userName = _cookieHelper.GetCookie("userid");
            int projectId = ProjectStatus.ProjectId;

            //Automapper
            //var prjSprintModelJs = _mappingService.Map<IList<ProjectSummaryViewModel>, IList<ProjectSummary>>(SprintDetails).ToList();

            IList<ProjectSummary> prjSprintModelJs = new List<ProjectSummary>();
            if (SprintDetails != null)
            {
                foreach (var model in SprintDetails)
                {
                    ProjectSummary prjSprintSingleRecord = new ProjectSummary();
                    prjSprintSingleRecord.ProjectSummaryId = model.ProjectSummaryId;
                    prjSprintSingleRecord.ProjectId = projectId;
                    prjSprintSingleRecord.ReleaseNumber = model.ReleaseNumber;
                    prjSprintSingleRecord.MileStoneId = model.MileStoneId;
                    prjSprintSingleRecord.SprintId = model.SprintId;
                    prjSprintSingleRecord.ReleaseDate = model.ReleaseDate;
                    prjSprintSingleRecord.StartDate = model.StartDate;
                    prjSprintSingleRecord.EndDate = model.EndDate;
                    prjSprintSingleRecord.FeedBack = model.FeedBack;
                    prjSprintSingleRecord.Remarks = model.Remarks;
                    prjSprintSingleRecord.WeekId = model.WeekId;
                    prjSprintSingleRecord.CreatedBy = model.CreatedBy;
                    prjSprintSingleRecord.CreatedDate = model.CreatedDate;
                    prjSprintSingleRecord.ModifiedBy = model.ModifiedBy;
                    prjSprintSingleRecord.ModifiedDate = model.ModifiedDate;
                    prjSprintModelJs.Add(prjSprintSingleRecord);
                }
            }

            //Automapper
            //var projectResourceModel = _mappingService.Map<IList<ProjectResourceUtilizationViewModel>, IList<ProjectResourceUtilization>>(ResourceDetails).ToList();

            // Resource Details
            IList<ProjectResourceUtilization> projectResourceModel = new List<ProjectResourceUtilization>();
            if (ResourceDetails != null)
            {
                foreach (var resourceDetail in ResourceDetails)
                {
                    ProjectResourceUtilization projectResourceSingleRecord = new ProjectResourceUtilization();

                    projectResourceSingleRecord.ProjectResourceId = resourceDetail.ProjectResourceId;
                    projectResourceSingleRecord.ProjectId = projectId;
                    projectResourceSingleRecord.WeekId = resourceDetail.WeekId;
                    projectResourceSingleRecord.RoleName = resourceDetail.RoleName;
                    projectResourceSingleRecord.ResourceName = resourceDetail.ResourceName;
                    projectResourceSingleRecord.AvailableEfforts = resourceDetail.AvailableEfforts;
                    projectResourceSingleRecord.ConsumedEfforts = resourceDetail.ConsumedEfforts;
                    projectResourceSingleRecord.Status = resourceDetail.Status;
                    projectResourceSingleRecord.CreatedBy = resourceDetail.CreatedBy;
                    projectResourceSingleRecord.CreatedDate = resourceDetail.CreatedDate;
                    projectResourceSingleRecord.ModifiedBy = resourceDetail.ModifiedBy;
                    projectResourceSingleRecord.ModifiedDate = resourceDetail.ModifiedDate;

                    projectResourceModel.Add(projectResourceSingleRecord);
                }
            }

            //Automapper
            //var projectResComplaintModel = _mappingService.Map<IList<ProjectComplaintViewModel>, IList<ProjectComplaint>>(ComplaintDetails).ToList();

            //Complaint Details
            IList<ProjectComplaint> projectResComplaintModel = new List<ProjectComplaint>();
            if (ComplaintDetails != null)
            {
                foreach (var complaintDetail in ComplaintDetails)
                {
                    ProjectComplaint projectComplaintSingleRecord = new ProjectComplaint();

                    projectComplaintSingleRecord.ComplaintId = complaintDetail.ComplaintId;
                    projectComplaintSingleRecord.ProjectId = projectId;
                    projectComplaintSingleRecord.WeekId = complaintDetail.WeekId;
                    projectComplaintSingleRecord.Remarks = complaintDetail.Remarks;
                    projectComplaintSingleRecord.StatusId = complaintDetail.StatusId;
                    projectComplaintSingleRecord.Description = complaintDetail.Description;
                    projectComplaintSingleRecord.PlannedAction = complaintDetail.PlannedAction;
                    projectComplaintSingleRecord.CreatedBy = complaintDetail.CreatedBy;
                    projectComplaintSingleRecord.CreatedDate = complaintDetail.CreatedDate;
                    projectComplaintSingleRecord.ModifiedBy = complaintDetail.ModifiedBy;
                    projectComplaintSingleRecord.ModifiedDate = complaintDetail.ModifiedDate;

                    projectResComplaintModel.Add(projectComplaintSingleRecord);
                }
            }

            // Payment Details
            IList<PaymentDetails> paymentDetailsModel = new List<PaymentDetails>();
            if (PaymentDetails != null)
            {
                foreach (var paymentDetail in PaymentDetails)
                {
                    PaymentDetails paymentDetailsSingleRecord = new PaymentDetails();

                    paymentDetailsSingleRecord.PaymentDetailId = paymentDetail.PaymentDetailId;
                    paymentDetailsSingleRecord.ProjectId = projectId;
                    paymentDetailsSingleRecord.WeekId = paymentDetail.WeekId;
                    paymentDetailsSingleRecord.InvoiceNumber = paymentDetail.InvoiceNumber;
                    paymentDetailsSingleRecord.InvoicedEffort = paymentDetail.InvoicedEffort;
                    paymentDetailsSingleRecord.InvoiceStatusId = paymentDetail.InvoiceStatusId;
                    paymentDetailsSingleRecord.CreatedBy = paymentDetail.CreatedBy;
                    paymentDetailsSingleRecord.CreatedDate = paymentDetail.CreatedDate;
                    paymentDetailsSingleRecord.ModifiedBy = paymentDetail.ModifiedBy;
                    paymentDetailsSingleRecord.ModifiedDate = paymentDetail.ModifiedDate;

                    paymentDetailsModel.Add(paymentDetailsSingleRecord);
                }
            }

            // Change Request Details
            IList<ChangeRequestDetails> changeRequestDetailsModel = new List<ChangeRequestDetails>();
            if (ChangeRequestDetails != null)
            {
                foreach (var changeRequestDetail in ChangeRequestDetails)
                {
                    ChangeRequestDetails changeRequestDetailsSingleRecord = new ChangeRequestDetails();

                    changeRequestDetailsSingleRecord.ChangeRequestId = changeRequestDetail.ChangeRequestId;
                    changeRequestDetailsSingleRecord.ProjectId = projectId;
                    changeRequestDetailsSingleRecord.WeekId = changeRequestDetail.WeekId;
                    changeRequestDetailsSingleRecord.ChangeRequestNumber = changeRequestDetail.ChangeRequestNumber;
                    changeRequestDetailsSingleRecord.ReceivedDate = changeRequestDetail.ReceivedDate;
                    changeRequestDetailsSingleRecord.ChangeRequestStatusId = changeRequestDetail.ChangeRequestStatusId;
                    changeRequestDetailsSingleRecord.CreatedBy = changeRequestDetail.CreatedBy;
                    changeRequestDetailsSingleRecord.CreatedDate = changeRequestDetail.CreatedDate;
                    changeRequestDetailsSingleRecord.ModifiedBy = changeRequestDetail.ModifiedBy;
                    changeRequestDetailsSingleRecord.ModifiedDate = changeRequestDetail.ModifiedDate;

                    changeRequestDetailsModel.Add(changeRequestDetailsSingleRecord);
                }
            }

            // Infrastructure Details
            IList<InfrastructureDetails> infrastructureDetailsModel = new List<InfrastructureDetails>();
            if (InfrastructureDetails != null)
            {
                foreach (var infrastructureDetail in InfrastructureDetails)
                {
                    InfrastructureDetails infrastructureDetailsSingleRecord = new InfrastructureDetails();

                    infrastructureDetailsSingleRecord.InfrastructureDetailId = infrastructureDetail.InfrastructureDetailId;
                    infrastructureDetailsSingleRecord.ProjectId = projectId;
                    infrastructureDetailsSingleRecord.WeekId = infrastructureDetail.WeekId;
                    infrastructureDetailsSingleRecord.DevelopmentAndQA = infrastructureDetail.DevelopmentAndQA;
                    infrastructureDetailsSingleRecord.UAT = infrastructureDetail.UAT;
                    infrastructureDetailsSingleRecord.Production = infrastructureDetail.Production;
                    infrastructureDetailsSingleRecord.CreatedBy = infrastructureDetail.CreatedBy;
                    infrastructureDetailsSingleRecord.CreatedDate = infrastructureDetail.CreatedDate;
                    infrastructureDetailsSingleRecord.ModifiedBy = infrastructureDetail.ModifiedBy;
                    infrastructureDetailsSingleRecord.ModifiedDate = infrastructureDetail.ModifiedDate;

                    infrastructureDetailsModel.Add(infrastructureDetailsSingleRecord);
                }
            }

            //To check with automapper
            //var prjSprintModel = _mappingService.Map<IList<ProjectSummaryViewModel>, IList<ProjectSummaryModel>>(prjSprintModelJs);

            int result = _projectSummaryService.SaveProjectSummary(ProjectStatus, prjSprintModelJs, projectResourceModel,
                projectResComplaintModel, paymentDetailsModel, changeRequestDetailsModel, infrastructureDetailsModel, userName, tabsPosted);

            //////common funtion needed
            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            //Bind Summary details
            projectMainViewModel.ProjectViewModel = GetProjectSummaryDetails(projectId, 0);

            if (result == 1)
            {
                projectMainViewModel.IsSuccess = Constants.SuccessMessage;
            }
            else
            {
                projectMainViewModel.IsSuccess = Constants.FailureMessage;

            }

            _itrackerHub.UpdateDashboard();
            //_itrackerHub.UpdateUserDashboard();
            
            return Json(projectMainViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadData(int WeekId)
        {
            string fileName = string.Empty;
            string destinationPath = string.Empty;
            var uniqueGUID = Guid.NewGuid();

            var file_Uploader = Request.Files["UploadedImage"];
           
            if (file_Uploader != null)
            {                                
                fileName = Path.GetFileName(file_Uploader.FileName);
                destinationPath = Path.Combine(Server.MapPath("~/Upload/"), uniqueGUID + "_" + fileName);
                file_Uploader.SaveAs(destinationPath);
            }

            string userName = _cookieHelper.GetCookie("userid");
            //string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Upload/File.xlsx");
            string sheetNameFirst = "Summary";
            var bookFirst = new LinqToExcel.ExcelQueryFactory(destinationPath);
            var dataFirstTab = from x in bookFirst.Worksheet(sheetNameFirst.TrimEnd())
                               select x;

            var worksheetsList = bookFirst.GetWorksheetNames().ToList();

            var dataSecondTab = from y in bookFirst.Worksheet(worksheetsList[0])
                                select y;

            int error = this._projectSummaryService.SaveDataFromExcelSheet(worksheetsList, dataFirstTab, dataSecondTab, WeekId, userName, destinationPath);
            if (error == -1)
            {
                return Json(new { Message = "Failure,First Tab Excel Format Is Not Correct" });
            }
            else if (error == -2)
            {
                return Json(new { Message = "Failure" });
            }
            else
            {
                return Json(new { Message = "Success" });
            }
        }
        
        public ActionResult GetProjectResourceDetails([DataSourceRequest] DataSourceRequest request, int projectId, int WeekId)
        {
            var projectMainViewModel = new ProjectMainViewModel();
            var prjResourceModel = _projectResourceService.GetProjectResources(projectId, WeekId).ToList();

            //Automapper
            var prjResourceViewModels = _mappingService.Map<IList<ProjectResourceUtilization>, IList<ProjectResourceUtilizationViewModel>>(prjResourceModel).ToList();

            projectMainViewModel.ProjectResouceUtilizationViewModel = prjResourceViewModels;

            return Json(projectMainViewModel.ProjectResouceUtilizationViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadPage(string pageName, string username = "")
        {
            var partialPage = string.Empty;

            ViewData["Status"] = this._projectService.AllStatus;

            if (pageName == "userProject")
            {
                var projectViewModel = new ProjectViewModel();
                projectViewModel.UserName = username;

                partialPage = "_UserProject";
                return PartialView(partialPage, projectViewModel);
            }

            if (pageName == "upload")
            {
                int currentWeek = this._genericService.GetWeek(DateTime.Now.GetPreviousWeek());
                IList<WeekModel> getWeekList = this._genericService.PrepareWeekList(currentWeek);
                ViewBag.WeekList = new SelectList(getWeekList, "Text", "Number");
                partialPage = "_Upload";
            }

            if (pageName == "dashboard")
            {
                var projectStatusReportViewModelList = LoadDashboardData();

                partialPage = "_Dashboard";
                return PartialView(partialPage, projectStatusReportViewModelList);
            }

            return PartialView(partialPage);
        }

        public ActionResult GetComplaintsDetails([DataSourceRequest] DataSourceRequest request, int projectId, int WeekId)
        {
            var projectMainViewModel = new ProjectMainViewModel();
            var prjComplaintModel = _projectComplaintService.GetProjectComplaints(projectId, WeekId).ToList();
            var prjComplaintViewModels = _mappingService.Map<IList<ProjectComplaint>, IList<ProjectComplaintViewModel>>(prjComplaintModel);
            projectMainViewModel.ProjectComplaintViewModel = prjComplaintViewModels;

            List<SelectListItem> lst = Extensions.EnumToSelectList(typeof(StatusList));
            //Filter enum 
            foreach (var item in prjComplaintViewModels)
            {
                var statusName = lst.Find(x => x.Value == item.StatusId.ToString());
                int statusValue = Convert.ToInt16(statusName.Value);

                switch (statusValue)
                {
                    case 0:
                        item.ComplaintStatus = StatusList.Open;
                        break;
                    case 1:
                        item.ComplaintStatus = StatusList.Closed;
                        break;
                    case 2:
                        item.ComplaintStatus = StatusList.Hold;
                        break;
                }

            }

            return Json(projectMainViewModel.ProjectComplaintViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPaymentDetails([DataSourceRequest] DataSourceRequest request, int projectId, int WeekId)
        {
            var projectMainViewModel = new ProjectMainViewModel();
            var prjPaymentDetailsModel = _paymentDetailsService.GetPaymentDetails(projectId, WeekId).ToList();

            var prjPaymentDetailsViewModels = _mappingService.Map<IList<PaymentDetails>, IList<PaymentDetailsViewModel>>(prjPaymentDetailsModel);

            projectMainViewModel.PaymentDetailsViewModel = prjPaymentDetailsViewModels;

            List<SelectListItem> lst = Extensions.EnumToSelectList(typeof(InvoiceStatus));
            //Filter enum 
            foreach (var item in prjPaymentDetailsViewModels)
            {
                var statusName = lst.Find(x => x.Value == item.InvoiceStatusId.ToString());
                int statusValue = Convert.ToInt16(statusName.Value);

                switch (statusValue)
                {
                    case 0:

                        item.InvoiceStatus = InvoiceStatus.Raised;
                        break;

                    case 1:
                        item.InvoiceStatus = InvoiceStatus.Pending;
                        break;

                    case 2:
                        item.InvoiceStatus = InvoiceStatus.Hold;
                        break;

                    case 3:
                        item.InvoiceStatus = InvoiceStatus.Received;
                        break;
                }
            }

            return Json(projectMainViewModel.PaymentDetailsViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChangeRequestDetails([DataSourceRequest] DataSourceRequest request, int projectId, int WeekId)
        {
            var projectMainViewModel = new ProjectMainViewModel();
            var prjChangeRequestDetailsModel = _changeRequestDetailsService.GetChangeRequestDetails(projectId, WeekId).ToList();

            // Have to replace by AutoMapper
            var prjChangeRequestDetailsViewModels = new List<ChangeRequestDetailsViewModel>();

            foreach (var item in prjChangeRequestDetailsModel)
            {
                var prjChangeRequestDetailsViewModel = new ChangeRequestDetailsViewModel
                {
                    ProjectId = projectId,
                    ChangeRequestId = item.ChangeRequestId,
                    ChangeRequestNumber = item.ChangeRequestNumber,
                    ReceivedDate = item.ReceivedDate,
                    ChangeRequestStatusId = item.ChangeRequestStatusId,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate
                };

                prjChangeRequestDetailsViewModels.Add(prjChangeRequestDetailsViewModel);
            }

            projectMainViewModel.ChangeRequestDetailsViewModel = prjChangeRequestDetailsViewModels;

            List<SelectListItem> lst = Extensions.EnumToSelectList(typeof(StatusList));
            //Filter enum 
            foreach (var item in prjChangeRequestDetailsModel)
            {
                var changeRequestStatusName = lst.Find(x => x.Value == item.ChangeRequestStatusId.ToString());
                int statusValue = Convert.ToInt16(changeRequestStatusName.Value);

                switch (statusValue)
                {
                    case 0:
                        item.ChangeRequestStatusList = StatusList.Open;
                        break;
                    case 1:
                        item.ChangeRequestStatusList = StatusList.Closed;
                        break;
                    case 2:
                        item.ChangeRequestStatusList = StatusList.Hold;
                        break;
                }
            }

            return Json(projectMainViewModel.ChangeRequestDetailsViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfrastructureDetails([DataSourceRequest] DataSourceRequest request, int projectId, int WeekId)
        {
            var projectMainViewModel = new ProjectMainViewModel();
            var prjInfrastructureDetailsViewModel = _infrastructureDetailsService.GetInfrastructureDetails(projectId, WeekId).ToList();

            // Have to replace by AutoMapper
            var prjInfrastructureDetailsViewModels = new List<InfrastructureDetailsViewModel>();

            foreach (var item in prjInfrastructureDetailsViewModel)
            {
                var prjPaymentDetailsViewModel = new InfrastructureDetailsViewModel
                {
                    ProjectId = projectId,
                    InfrastructureDetailId = item.InfrastructureDetailId,
                    DevelopmentAndQA = item.DevelopmentAndQA,
                    UAT = item.UAT,
                    Production = item.Production,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate
                };

                prjInfrastructureDetailsViewModels.Add(prjPaymentDetailsViewModel);
            }

            projectMainViewModel.InfrastructureDetailsViewModel = prjInfrastructureDetailsViewModels;

            return Json(projectMainViewModel.InfrastructureDetailsViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SPRINT DETAILS
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private IList<ProjectSummaryViewModel> GetSprintDetails(int projectId, int WeekId)
        {
            var prjSummaryModel = _projectSummaryService.GetProjectSprintDetails(projectId, WeekId).ToList();
            var prjSummaryViewModels = _mappingService.Map<IList<ProjectSummary>, IList<ProjectSummaryViewModel>>(prjSummaryModel);
            return prjSummaryViewModels;
        }

        private ProjectViewModel GetProjectSummaryDetails(int projectId, int weekId = 0)
        {
            var project = this._projectService.GetProjectSummary(projectId);

            if (weekId == 0)
            {
                weekId = this._genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            }
            var projectStatus = this._projectService.GetProjectStatus(projectId, weekId);

            var projectViewModel = new ProjectViewModel();
            if (project != null)
            {
                projectViewModel.ProjectId = projectId;
                projectViewModel.ProjectName = project.ProjectName;
                projectViewModel.StartDate = project.StartDate;
                projectViewModel.PlannedEndDate = project.PlannedEndDate;
                projectViewModel.IsActive = project.IsActive;
                projectViewModel.ProjectDescription = project.ProjectDescription;

                if (projectStatus != null)
                {
                    projectViewModel.ProjectStatusId = projectStatus.ProjectStatusId;
                    projectViewModel.ProjectSummary = projectStatus.ProjectSummary;
                    projectViewModel.StatusId = projectStatus.StatusId;
                }
                projectViewModel.Status = new SelectList(this._projectService.AllStatus, "StatusId", "StatusName");
            }

            return projectViewModel;
        }

        private void PopulateDataIntoViewModel(ProjectMainViewModel ProjectMainViewModel)
        {
            int currentWeek = this._genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            IList<WeekModel> getWeekList = this._genericService.PrepareWeekList(currentWeek);
            ProjectMainViewModel.WeekList = new SelectList(getWeekList, "Text", "Number");
            //To do current week should be selected
            ProjectMainViewModel.CurrentWeek = 1;

            ProjectMainViewModel.ProjectViewModel = new ProjectViewModel();
            ProjectMainViewModel.ProjectViewModel.Status = new SelectList(this._projectService.AllStatus, "StatusId", "StatusName");
        }

        private List<ProjectStatusReportViewModel> LoadDashboardData()
        {
            string userName = _cookieHelper.GetCookie("userid");

            var projects = this._projectService.GetProjectsByUsername(userName).ToList();
            var projectStatusList = this._userDashboardService.GetUserProjectStatus(userName).ToList();
            WeekListData weekListData; //= new WeekListData();

            var projectStatusReportViewModelList = new List<ProjectStatusReportViewModel>();
            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int WeekId = _genericService.GetWeekIdFromMasterTable(currentWeek - 1, DateTime.Now.Year);

            foreach (var project in projects)
            {
                var projectData = new ProjectData();
                projectData.ProjectName = project.ProjectName;
                projectData.ProjectId = project.ProjectId;
                projectData.WeekData = new List<WeekData>();

                var weekList = new List<WeekListData>();

                foreach (var projectStatus in projectStatusList.Where(ps => ps.ProjectId == project.ProjectId).ToList())
                {
                    var weekInfo = _genericService.GetWeekNumberYearByWeekId(projectStatus.WeekId);

                    projectData.WeekData.Add(new WeekData { WeekId = projectStatus.WeekId, StatusId = projectStatus.StatusId });
                }

                for (int i = WeekId; i > 0; i--)
                {
                    var weekInfo = _genericService.GetWeekNumberYearByWeekId(i);

                    weekListData = new WeekListData()
                    {
                        WeekId = weekInfo.WeekId,
                        Year = weekInfo.Year,
                        WeekNumber = weekInfo.WeekNumber,
                        Text = weekInfo.Text
                    };

                    weekList.Add(weekListData);
                }

                var sortedWeekList = from element in weekList
                                     orderby element.Year descending
                                     select element;

                projectStatusReportViewModelList.Add(new ProjectStatusReportViewModel { ProjectData = projectData, CurrentWeek = WeekId, WeekListData = sortedWeekList.ToList() });
            }
            return projectStatusReportViewModelList;
        }

    }
}
