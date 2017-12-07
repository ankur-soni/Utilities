using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Models.JobVite;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Filters;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Web.Models.JobVite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    [CustomAuthorize("Admin", "Recruiter")]
    public class JobViteController : Controller
    {
        private readonly ITestSuiteService _testSuiteService;

        private readonly IEmailService _emailService;

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public JobViteController(IEmailService emailService, ITestSuiteService testSuiteService, IUserService userService)
        {
            _testSuiteService = testSuiteService;
            _emailService = emailService;
            //_userService = userService;

        }

        // GET: JobVite
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SyncCandidates()
        {
            StringBuilder syncLog = new StringBuilder();
            string JobViteBaseURL = ConfigurationManager.AppSettings["JobViteBaseURL"];
            string JobViteUserId = ConfigurationManager.AppSettings["JobViteUserId"];
            string JobVitesc = ConfigurationManager.AppSettings["JobVitesc"];
            string JobViteCandidateSelecttionStatus = ConfigurationManager.AppSettings["JobViteCandidateSelecttionStatus"];

            string baseAddress = JobViteBaseURL + "=" + JobViteUserId + "&sc=" + JobVitesc + "&wflowstate=" + JobViteCandidateSelecttionStatus + "&format=json";

            //string baseAddress = "https://api.jobvite.com/api/v2/candidate?api=silicustechnologies_candidate_api_key&sc=c4b68fcb2c29aba71c6a5c418e39e912&wflowstate=New&format=json";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    HttpResponseMessage response = await httpClient.GetAsync(baseAddress);
                    response.EnsureSuccessStatusCode();
                    syncLog.AppendLine("JobVite Connection Established");
                    string result = await response.Content.ReadAsStringAsync();
                    APIResponse businessunits = JsonConvert.DeserializeObject<APIResponse>(result);
                    var candidateList = (from candidate in businessunits.candidates

                                         select new UserViewModel()
                                         {
                                             FirstName = candidate.firstName,
                                             LastName = candidate.lastName,
                                             Email = candidate.email,
                                             CandidateStatus = candidate.application.workflowState,
                                             JobViteId = candidate.application.eId,
                                             Position = candidate.application.job.title
                                         }).ToList();

                    // return Json(result, JsonRequestBehavior.AllowGet);
                    return Json(new { Items = candidateList });
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AdUserforCandidates(UserViewModel candidate)
        {
            StringBuilder syncLog = new StringBuilder();
            try
            {

                if (candidate != null)
                {
                    if (candidate.Email == null)
                    {
                        syncLog.AppendLine("Skipping User for -" + candidate.FirstName + " " + candidate.LastName + "(" + candidate.JobViteId + ") as no valid email is present");
                    }
                    else
                    {
                        ApplicationUser alreadyuser = UserManager.FindByEmail(candidate.Email);

                        if (alreadyuser == null)
                        {
                            alreadyuser = await CreateUserMethod(candidate);
                            if (alreadyuser != null)
                            {
                                syncLog.AppendLine("User is created for -" + candidate.FirstName + " " + candidate.LastName + "(" + candidate.JobViteId + ")");
                            }
                        }

                        alreadyuser.CandidateStatus = candidate.CandidateStatus;
                        UserManager.Update(alreadyuser);
                    }
                }
                //syncLog.AppendLine("Sync Complete");

            }
            catch (Exception ex)
            {
                syncLog.Append(ex.Message);
                syncLog.Append(ex.InnerException);
                syncLog.Append(ex.StackTrace);
                return Json(syncLog.ToString(), JsonRequestBehavior.AllowGet);
            }
            //DataSourceResult cList = candidateList.ToDataSourceResult(request);
            return Json(syncLog.ToString(), JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetSystemCandidates([DataSourceRequest] DataSourceRequest request)
        {
            var users = UserManager.Users.ToList();
            var candidateList = (from candidate in users

                                 select new UserViewModel()
                                 {
                                     UserId = candidate.Id,
                                     FirstName = candidate.FirstName,
                                     LastName = candidate.LastName,
                                     Email = candidate.Email,
                                     CandidateStatus = candidate.CandidateStatus,
                                     JobViteId = candidate.JobViteId,
                                     Position = candidate.Position,
                                     NewPassword = candidate.PhoneNumber
                                 }).ToList();
            DataSourceResult cList = candidateList.ToDataSourceResult(request);
            return Json(cList);

        }

        public ActionResult AssignTest(string UserId)
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                var user = UserManager.FindById(UserId);
                var employeeTestSuit = _testSuiteService.GetEmployeeTestSuite().Where(x => x.StatusId == Convert.ToInt32(CandidateStatus.TestAssigned) && x.CandidateID == user.Id).FirstOrDefault();

                var model = new AssignTestViewModel()
                {
                    CandidateFirstName = user.FirstName,
                    CandidateLastName = user.LastName,
                    CandidateEmail = user.Email,
                    CandidateJobViteId = user.JobViteId,
                    CandidatePosition = user.Position,
                };

                if (employeeTestSuit != null)
                {
                    model.existingAssignedTest = employeeTestSuit.EmployeeTestSuiteId;
                    model.ReviewerId = employeeTestSuit.AssignedReviewers.Split(',');
                    model.TestSuiteId = employeeTestSuit.TestSuiteId;
                }

                return PartialView("AssignTest", model);
            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult> SaveCandidateAndAssignTest(AssignTestViewModel model)
        {
            var user = UserManager.FindByEmail(model.CandidateEmail);

            var employeeTestSuits = _testSuiteService.GetEmployeeTestSuite().Where(x => x.StatusId == Convert.ToInt32(CandidateStatus.TestAssigned) && x.CandidateID == user.Id).ToList();

            if (employeeTestSuits.Any())
            {
                foreach (var empTestSuite in employeeTestSuits)
                {
                    _testSuiteService.DeleteEmployeeTestSuite(empTestSuite);
                }
            }

            var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(m => m.TestSuiteId == model.TestSuiteId && !m.IsDeleted).SingleOrDefault();
            EmployeeTestSuite userTestSuite = new EmployeeTestSuite();
            userTestSuite.EmployeeId = 0;
            userTestSuite.CandidateID = user.Id;
            userTestSuite.TestSuiteId = model.TestSuiteId;
            userTestSuite.AssignedReviewers = String.Join(",", model.ReviewerId); 
            userTestSuite.StatusId = (int)CandidateStatus.TestAssigned;
            _testSuiteService.AssignEmployeeSuite(userTestSuite, testSuiteDetails);
            user.CandidateStatus = CandidateStatus.TestAssigned.ToString();
            UserManager.Update(user);
            return Json(1);
        }

        private async Task<ApplicationUser> CreateUserMethod(UserViewModel model)
        {
            // User 
            var Currentuser = UserManager.FindByEmail(model.Email);
            if (Currentuser != null)
                return Currentuser;
            else
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.JobViteId = model.JobViteId;
                user.Position = model.Position;
                user.CandidateStatus = model.CandidateStatus;
                user.Email = model.Email;
                //var NewPassword = model.FirstName.ToUpper() + model.LastName.ToLower() + "@123456";
                //var ConfirmPassword = model.FirstName.ToUpper() + model.LastName.ToLower() + "@123456";

                var NewPassword = System.Web.Security.Membership.GeneratePassword(8, 1);
                var ConfirmPassword = NewPassword;

                user.PhoneNumber = NewPassword;

                var userResult = await UserManager.CreateAsync(user, NewPassword);
                if (userResult.Succeeded)
                {

                    var result = await UserManager.AddToRoleAsync(user.Id, RoleName.Candidate.ToString());
                    if (result.Succeeded)
                    {

                    }

                }
                else
                {
                    ModelState.AddModelError("", userResult.Errors.First());

                }
                return user;
            }
        }
    }
}