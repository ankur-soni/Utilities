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
    [Authorize]
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

        public async Task<ActionResult> SyncCandidates()
        {
            StringBuilder syncLog = new StringBuilder();
            string baseAddress = "https://api.jobvite.com/api/v2/candidate?api=silicustechnologies_candidate_api_key&sc=c4b68fcb2c29aba71c6a5c418e39e912&wflowstate=New&format=json&city=Pune";
            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpResponseMessage response = await httpClient.GetAsync(baseAddress);
                response.EnsureSuccessStatusCode();
                syncLog.AppendLine("JobVite Connection Established");
                string result = await response.Content.ReadAsStringAsync();
                APIResponse businessunits = JsonConvert.DeserializeObject<APIResponse>(result);
                syncLog.AppendLine("Candidate List Received");
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

                foreach (var user in candidateList)
                {
                    ApplicationUser alreadyuser = UserManager.FindByEmail(user.Email);

                    if (alreadyuser == null)
                    {
                        alreadyuser = await CreateUserMethod(user);
                        if (alreadyuser != null)
                        {
                            syncLog.AppendLine("User is created for -" + user.FirstName + " " + user.LastName + "(" + user.JobViteId + ")");
                        }
                    }

                    alreadyuser.CandidateStatus = user.CandidateStatus;
                    UserManager.Update(alreadyuser);
                }
                syncLog.AppendLine("Sync Complete");
                //DataSourceResult cList = candidateList.ToDataSourceResult(request);
                return Json(syncLog.ToString(), JsonRequestBehavior.AllowGet);
            }

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
                                     Position = candidate.Position
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
                    model.ReviewerId = employeeTestSuit.ReviewerId.Value;
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
            userTestSuite.ReviewerId = model.ReviewerId;
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
                var NewPassword = model.FirstName.ToUpper() + model.LastName.ToLower() + "@123456";
                var ConfirmPassword = model.FirstName.ToUpper() + model.LastName.ToLower() + "@123456";

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