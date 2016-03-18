using System;
using System.Linq;
using Kendo.Mvc.UI;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Web.Models;
using Silicus.ProjectTracker.Web.Filters;
using Silicus.ProjectTracker.Web.ViewModel;
using Silicus.ProjectTracker.Core.Interfaces;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Web.UserMembership;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Web.Controllers
{
    public class AdminController : Controller
    {
        public readonly IProjectService _projectService;
        public readonly IProjectMappingService _projectMappingService;
        private readonly IEmailService emailService;
        private readonly ICookieHelper _cookieHelper;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IGenericService _genericService;
        private readonly IAdminDashboardService _dashboardService;
        private readonly IMappingService _mappingService;
        private readonly IActiveDirectoryService _iactiveDirectoryService;

        public AdminController(IMembershipService membershipService, ICookieHelper cookieHelper, IActiveDirectoryService activeDirectoryService,
            IProjectService projectService, IEmailService emailService, IProjectMappingService projectMappingService, IGenericService genericService,
            IAdminDashboardService dashboardService, IMappingService mappingService, IActiveDirectoryService iactiveDirectoryService)
        {
            this._projectService = projectService;
            this.emailService = emailService;
            this._cookieHelper = cookieHelper;
            this._activeDirectoryService = activeDirectoryService;
            this._projectMappingService = projectMappingService;
            this._genericService = genericService;
            this._dashboardService = dashboardService;
            this._mappingService = mappingService;
            this._iactiveDirectoryService = iactiveDirectoryService;
        }

        [AuthorizeADAttribute(Groups = "ProjectTrackerAdmin")]
        public ActionResult Dashboard()
        {
            var totalCounts = LoadDashboardData();

            return View(totalCounts);
        }

        public ActionResult GetProjects([DataSourceRequest] DataSourceRequest request)
        {
            var projects = this._projectService.GetProjects().ToList();

            var projectViewModels = new List<ProjectViewModel>() { };


            foreach (var project in projects)
            {
                var projectSummary = GetProjectSummaryDetails(project.ProjectId);
                projectViewModels.Add(projectSummary);
            }

            return Json(projectViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateProject()
        {
            var project = new Project();
            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            var projectSummary = new ProjectViewModel();
            //projectSummary.Status = new SelectList(this._projectService.AllStatus, "StatusId", "StatusName");
            return PartialView("_CreateProject", projectSummary);
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel projectViewModel, string StatusId)
        {
            string userName = _cookieHelper.GetCookie("userid");

            if (projectViewModel != null && ModelState.IsValid)
            {
                var project = new Project()
                {
                    ProjectId = projectViewModel.ProjectId,
                    ProjectName = projectViewModel.ProjectName,
                    StartDate = projectViewModel.StartDate,
                    PlannedEndDate = projectViewModel.PlannedEndDate,
                    IsActive = projectViewModel.IsActive,
                    ProjectDescription = projectViewModel.ProjectDescription
                };

                var projectMainViewModel = new ProjectMainViewModel();
                PopulateDataIntoViewModel(projectMainViewModel);

                int projectId = this._projectService.AddProject(project, userName);
                                
                if (projectId == 0)
                {
                    ModelState.AddModelError("DuplicateProjectName", "Please enter a unique name for the project.");
                    return PartialView("_CreateProject", projectViewModel);
                }
                else if (projectId == -1)
                {
                    ModelState.AddModelError("SaveError", "Error in Saving Data.");
                    return PartialView("_CreateProject", projectViewModel);

                }

                return Json(new { Status = "Success", Message = "Project created successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("_CreateProject", projectViewModel);
            }
        }

        [HttpGet]
        public ActionResult EditProject(int projectId)
        {
            var project = this._projectService.GetProjectById(projectId);

            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);

            var projectSummary = GetProjectSummaryDetails(projectId);
            
            return PartialView("_CreateProject", projectSummary);
        }

        [HttpPost]
        public ActionResult UpdateProject(ProjectViewModel projectViewModel)
        {
            string userName = _cookieHelper.GetCookie("userid");

            if (projectViewModel != null && ModelState.IsValid)
            {
                var project = new Project()
                {
                    ProjectId = projectViewModel.ProjectId,
                    ProjectName = projectViewModel.ProjectName,
                    StartDate = projectViewModel.StartDate,
                    PlannedEndDate = projectViewModel.PlannedEndDate,
                    IsActive = projectViewModel.IsActive,
                    ProjectDescription = projectViewModel.ProjectDescription
                };

                int projectId = this._projectService.UpdateProject(project, userName);
                                
                //projectViewModel.Status = new SelectList(this._projectService.AllStatus, "StatusId", "StatusName");

                var projectMainViewModel = new ProjectMainViewModel();
                PopulateDataIntoViewModel(projectMainViewModel);

                if (projectId == -1)
                {
                    ModelState.AddModelError("DuplicateProjectName", "Please enter a unique name for the project.");

                    return PartialView("_CreateProject", projectViewModel);
                }

                return Json(new { Status = "Success", Message = "Project updated successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("_CreateProject", projectViewModel);
            }
        }

        [HttpPost]
        public void DeleteProject(Project project)
        {
            string userName = _cookieHelper.GetCookie("userid");

            if (project != null)
            {
                this._projectService.DeleteProject(project, userName);
            }

            var projectMainViewModel = new ProjectMainViewModel();
            PopulateDataIntoViewModel(projectMainViewModel);
        }

        /// <summary>
        /// Project Mapping
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProjectMappingData(string userName)
        {
            var viewModel = new MoverBoxModel
            {
                AvailableItems = new List<SelectableItemModel>(),
                SelectedItems = new List<SelectableItemModel>()
            };

            var availableItems = _projectService.GetProjectList().ToList();
            var selectedItems = _projectMappingService.GetAssignedProjects(userName);

            viewModel.AvailableItems = availableItems.Select(a => new SelectableItemModel
            {
                ProjectName = a.ProjectName,
                ProjectId = a.ProjectId,
                IsSelected = selectedItems.Contains(a.ProjectId.ToString()),
                IsActive = a.IsActive,
                IsAssigned = _projectMappingService.IsProjectMapped(a.ProjectId, userName)
            }).OrderBy(a => a.ProjectName).ToList();

            viewModel.SelectedItems = selectedItems.Select(s => new SelectableItemModel
            {
                ProjectName = s
                
            }).ToList();

            return Json((viewModel), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignProjectsToUser(string projectsList, string user)
        {
            string userName = user;
            var projectList = projectsList.Split('~').ToList();
            _projectMappingService.SaveProjectsToUser(projectList, userName);

            var adUsers = this._activeDirectoryService.GetActiveDirectoryUsers("User");
            ViewBag.ActiveDirectoryUsers = new SelectList(adUsers, "UserName", "DisplayName", "adUsersControl");

            return PartialView("_Mapping");
        }

        public ActionResult LoadPage(string pageName)
        {
            var partialPage = string.Empty;

            ViewData["Status"] = this._projectService.AllStatus;

            if (pageName == "adminProject")
            {
                partialPage = "_AdminProject";
            }
            
            if (pageName == "dashboard")
            {
                partialPage = "_Dashboard";
                var totalCounts = LoadDashboardData();

                return PartialView(partialPage, totalCounts.ProjectStatusAdminDashBoardModel);
            }

            if (pageName == "mapping")
            {
                var adUsers = this._activeDirectoryService.GetActiveDirectoryUsers("User");
                ViewBag.ActiveDirectoryUsers = new SelectList(adUsers, "UserName", "DisplayName", "adUsersControl");
                partialPage = "_Mapping";
            }

            if (pageName == "userlist")
            {
                partialPage = "_Userlist";
            }

            return PartialView(partialPage);
        }

        private static string GenerateEncodedKey(string username, string guid)
        {
            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + "new" + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
            return HashParams;
        }

        // For future use -  Don't delete
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SendEmail(FormCollection email)
        //{           
        //    string retVal = "failed";
        //    if (!string.IsNullOrEmpty(email[1]))
        //    {
        //        var userDetails = membershipService.GetUserDetails(email[1]);
        //        if (SendWelcomeMail(userDetails.Email, email[1], userDetails.ProviderUserKey))
        //        {
        //            retVal = "succeeded";
        //        }
        //        }

        //    return Json(retVal);
        //}

        private bool SendWelcomeMail(string email, string userFirstName, object key)
        {
            bool retVal = false;
            try
            {
                string urlEncodedUserName = System.Web.HttpUtility.UrlEncode(email); // url encoded
                string subject = ConfigurationManager.AppSettings["ProductNameLong"] + ": " +
                                 ConfigurationManager.AppSettings["SmtpMailSubjectWelcome"];
                string baseUrl = ConfigurationManager.AppSettings["SmtpMailbaseUrl"];

                string link = baseUrl + "/Account/ResetPassword/?username=" + urlEncodedUserName +
                              "&reset=" + GenerateEncodedKey(email, key.ToString());

                string body =
                    "<html>" +
                    "<body>" +
                    "<table style='width: 630px; border: none;'><tr><td><table style='border: 1px solid #5C666F; align: left; width: 630px; font-family: arial; font-size: 14px; height: auto;border-spacing: 0;'>" +
                    "<tr style='width: 630px; height: 44px; border-bottom: 1px solid #5C666F;'>" +
                    "<td style=' background-color: #00263D; height: 44px; width: 195px; border-bottom: 5px solid #55A51C; margin: 0 auto;'><img src='" +
                    ConfigurationManager.AppSettings["WebsiteURL"] +
                    "/Images/rigdig_logo_email.png' style='padding: 3px; margin: 0 auto;' /></td>" +
                    "<td style='background-color: #5C666F; width: 435px; height: 44px; border-bottom: 5px solid #A3A9AC;vertical-align: middle;'>" +
                    "<p style='font-size: 19px; margin-left: 20px; color: #fff; font-weight: bold; padding: 0; width: 100%;'>" +
                    "Welcome to " + ConfigurationManager.AppSettings["ProductNameLong"] + "</p>" +
                    "</td>" +
                    "</tr>" +
                    "<tr>" +
                    "<td colspan='2' style='width: 630px;width: auto; border: 4px solid #D1D3D4; border-top: none; padding: 30px; margin-top: 4px;'>" +
                    "<p style='font-size: 14px; color: #000; margin-top: 20px!important;'>" +
                    "Dear " + userFirstName + "," +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000; margin-bottom: 30px;'>" +
                    "Welcome to " + ConfigurationManager.AppSettings["ProductNameLong"] +
                    "! Your account contains a wealth of information to help guide your strategic planning, target new prospects, retain customers and better understand your market." +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000; margin-bottom: 30px;'>" +
                    "<span style='color: #55A51C; font-weight: bold;'>" +
                    "Access " + ConfigurationManager.AppSettings["ProductNameLong"] + "</span>" +
                    "<br />" +
                    "You can begin accessing " + ConfigurationManager.AppSettings["ProductNameShort"] +
                    " with the following login credentials:" +
                    "</p>" +
                    "<ul>" +
                    "<li style='list-style-type:disc; width: 470px; text-decoration: none !important; font-family: arial; font-size: 14px; color: #000; '>" +
                    "Username: <span style='color: #00698E; text-decoration: none !important;'>" + email +
                    "</span></li>" +
                    "<li style='list-style-type:disc; width: 470px;'>" +
                    "Click to set your Password: " +
                    "<a href='" + link +
                    "' target='_blank' style='display: inline; width: 450px; -ms-word-wrap:break-word; word-wrap:break-word; color: #00698E; text-decoration: underline;'>" +
                    ConfigurationManager.AppSettings["ProductNameShort"] + "_PasswordSetup" +
                    "</a>" +
                    "</li>" +
                    "</ul>" +
                    "<p style='font-size: 14px; color: #000; margin-top: 30px;'>" +
                    "<span style='color: #55A51C; font-weight: bold;'>" +
                    ConfigurationManager.AppSettings["ProductNameShort"] + " Client Success Team" +
                    "</span>" +
                    "<br />" +
                    "We look forward to helping you put the power of " +
                    ConfigurationManager.AppSettings["ProductNameShort"] +
                    " Business Intelligence to work for your organization." +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000;'>" +
                    "Please don’t hesitate to contact us with any questions." +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000;'>" +
                    "Kind regards," +
                    "</p>" +
                    "<p style='font-size: 14px; color: #000; margin-bottom: 30px; margin-bottom: 10px !important;'>" +
                    "<span style='font-weight: bold;'>" +
                    ConfigurationManager.AppSettings["ProductNameLong"] + " Client Success Team" +
                    "</span>" +
                    "<br /><span style='color: #000; text-decoration: none!important; font-size: 14px;'>" +
                    ConfigurationManager.AppSettings["SmtpMailSupportAddress"] + "" +
                    "</span><br />" +
                    ConfigurationManager.AppSettings["ContactPhone_DotFormat"] +
                    "</p>" +
                    "<p style='text-align:right;margin-bottom: -15px; margin-right: -10px'><img src='" +
                    ConfigurationManager.AppSettings["WebsiteURL"] +
                    "/Images/RandallReillyProduct.png' style='padding: 2px;' /></p></td>" +
                    "</tr></table>" +
                    "<table style='border: none; width: 630px; border-spacing: 0; align: left; margin: 0; padding: 0;'><tr><td colspan='2' style='border-top: 5px solid #00263D;'>" +
                    "<p style='color: #000; padding: 10px; margin-top: 10px; font-family: arial; font-size: 12px;'>" +
                    "NEED HELP?: Training and reference materials are available at " +
                    "<a href='http://www.silicus.com/' target='_blank' style='color: #00698E; text-decoration: underline;'>" +
                    "http://www.silicus.com/" +
                    "</a>" +
                    "<br />" +
                    "CONTACT US: We welcome your questions and comments. You can reach us at " +
                    "<a href='mailto:" + ConfigurationManager.AppSettings["SmtpMailSupportAddress"] +
                    "' target='_blank' style='color: #00698E; text-decoration: underline;'>" +
                    ConfigurationManager.AppSettings["SmtpMailSupportAddress"] + "" +
                    "</a>" +
                    "</p>" +
                    "</td></tr></table></td></tr></table>" +
                    "</body>" +
                    "</html>";

                emailService.SendEmailAsync(email, subject, body);
                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
                System.Diagnostics.Trace.WriteLine(ex);
            }
            return retVal;
        }

        private ProjectViewModel GetProjectSummaryDetails(int projectId = 0, int weekId = 0)
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
                //Have to use AutoMapper..in future
                projectViewModel.ProjectId = projectId;
                projectViewModel.ProjectName = project.ProjectName;
                projectViewModel.StartDate = project.StartDate;
                projectViewModel.PlannedEndDate = project.PlannedEndDate;
                projectViewModel.IsActive = project.IsActive;
                projectViewModel.ProjectDescription = project.ProjectDescription;

                if (projectStatus != null)
                {
                    projectViewModel.ProjectStatusId = projectStatus.ProjectStatusId;
                    //projectViewModel.ProjectDescription = projectStatus.ProjectDescription;
                    projectViewModel.StatusId = projectStatus.StatusId;
                }
                projectViewModel.Status = new SelectList(this._projectService.AllStatus, "StatusId", "StatusName");
            }
            return projectViewModel;

        }

        private void PopulateDataIntoViewModel(ProjectMainViewModel ProjectMainViewModel)
        {
            ProjectMainViewModel.ProjectViewModel = new ProjectViewModel();
            ProjectMainViewModel.ProjectViewModel.Status = new SelectList(this._projectService.AllStatus, "StatusId", "StatusName");
        }

        [HttpPost]
        public ActionResult GetProjectStatusDataForPieChart()
        {
            var model = _dashboardService.GetProjectStatusDataForPieChart();
            var prjstatusPieChartViewModel = _mappingService.Map<IList<ProjectStatusPieChartModel>, IList<ProjectStatusPieChartViewModel>>(model);
            return Json(prjstatusPieChartViewModel);

        }

        [HttpPost]
        public ActionResult GetDataForDefaulterList([DataSourceRequest] DataSourceRequest request)
        {
            var model = _dashboardService.GetDataForDefaulterList();
            var prjdefaulterViewModel = _mappingService.Map<IList<ProjectTopDefaultersModel>, IList<ProjectTopDefaultersViewModel>>(model);
            return Json(prjdefaulterViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetListForStatusReportSubmitted([DataSourceRequest] DataSourceRequest request)
        {
            var model = _dashboardService.GetListForStatusReportSubmitted();
            var prjdefaulterViewModel = _mappingService.Map<IList<ProjectTopSubmittedModel>, IList<ProjectTopSubmittedViewModel>>(model);
            return Json(prjdefaulterViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetManagers([DataSourceRequest] DataSourceRequest request)
        {
            var userListViewModel = new List<UserListViewModel>();
            var adUsers = this._activeDirectoryService.GetActiveDirectoryUsers("User");

            foreach (var manager in adUsers)
            {
                var userList = new UserListViewModel();

                userList.UserName = manager.UserName;
                userList.UserDisplayName = manager.DisplayName;
                userList.ProjectCount = _projectMappingService.GetAssignedProjects(Convert.ToString(manager.DisplayName)).Count();

                userListViewModel.Add(userList);
            }

            return Json(userListViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private ProjectsTotalCountsViewModel LoadDashboardData()
        {
            ViewBag.AdminMenuVisibilty = true;
            var totalCounts = new ProjectsTotalCountsViewModel();
            totalCounts.NoOfAssignedProjects = this._projectMappingService.GetCountsOfAssignedProject();
            totalCounts.NoOfManagers = this._iactiveDirectoryService.GetActiveDirectoryUsers("User").Count();
            totalCounts.NoOfProjects = this._projectService.GetProjects().Count();
            totalCounts.NoOfUnAssignedProjects = this._projectMappingService.GetCountsOfNonAssignedProject();
            totalCounts.ProjectStatusAdminDashBoardModel = new List<ProjectStatusAdminDashBoardModel>();

            var projectStatusAdminDashBoardModel = new ProjectStatusAdminDashBoardModel();
            
            var currentWeek = _genericService.GetWeek(DateTime.Now.GetPreviousWeek());
            int weekId = _genericService.GetWeekIdFromMasterTable(currentWeek - 1, DateTime.Now.Year);
            var weekInfo = _genericService.GetWeekNumberYearByWeekId(weekId);

            var weekListData = new WeekListData()
            {
                Year = weekInfo.Year,
                WeekNumber = weekInfo.WeekNumber,
                Text = weekInfo.Text
            };

            projectStatusAdminDashBoardModel.WeekListData = weekListData;

            totalCounts.ProjectStatusAdminDashBoardModel.Add(projectStatusAdminDashBoardModel);
            return totalCounts;
        }
    }
}
