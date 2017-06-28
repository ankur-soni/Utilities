using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
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

        public async Task<ActionResult> GetCandidates([DataSourceRequest] DataSourceRequest request)
        {
            string baseAddress = "https://api.jobvite.com/api/v2/candidate?api=silicustechnologies_candidate_api_key&sc=c4b68fcb2c29aba71c6a5c418e39e912&wflowstate=New&format=json&city=Pune";
            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpResponseMessage response = await httpClient.GetAsync(baseAddress);
                response.EnsureSuccessStatusCode();
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

                DataSourceResult cList = candidateList.ToDataSourceResult(request);
                return Json(cList);
            }

        }

        public ActionResult AssignTest(string FirstName, string lastName, string Email, string jobviteId, string position)
        {
            var model = new AssignTestViewModel()
            {
                CandidateFirstName = FirstName,
                CandidateLastName = lastName,
                CandidateEmail = Email,
                CandidateJobViteId = jobviteId,
                CandidatePosition = position,
            };
            return PartialView("AssignTest", model);
        }
        [HttpPost]
        public async Task<ActionResult> SaveCandidateAndAssignTest(AssignTestViewModel model)
        {
            var user = await CreateUserMethod(model);

            var employeeTestSuits = _testSuiteService.GetEmployeeTestSuite().Where(x => x.TestSuiteId == model.TestSuiteId && x.StatusId == Convert.ToInt32(CandidateStatus.TestAssigned) && x.CandidateID == user.Id).ToList();

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
            userTestSuite.StatusId = (int)CandidateStatus.TestAssigned;
            _testSuiteService.AssignEmployeeSuite(userTestSuite, testSuiteDetails);

            return Json(1);
        }


        private async Task<ApplicationUser> CreateUserMethod(AssignTestViewModel model)
        {
            // User 
            var Currentuser = UserManager.FindByEmail(model.CandidateEmail);
            if (Currentuser != null)
                return Currentuser;
            else
            {
                var user = new ApplicationUser { UserName = model.CandidateEmail, Email = model.CandidateEmail };
                //if (vuser.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
                //{
                //    vuser.TestStatus = CandidateStatus.New.ToString();
                //    vuser.CandidateStatus = CandidateStatus.New.ToString();
                //}
                user.FirstName = model.CandidateFirstName;
                user.LastName = model.CandidateLastName;
                user.JobViteId = model.CandidateJobViteId;
                user.Position = model.CandidatePosition;
                user.Email = model.CandidateEmail;
                var NewPassword = model.CandidateFirstName.ToUpper() + model.CandidateLastName.ToLower() + "@123456";
                var ConfirmPassword = model.CandidateFirstName.ToUpper() + model.CandidateLastName.ToLower() + "@123456";

                var userResult = await UserManager.CreateAsync(user, NewPassword);
                if (userResult.Succeeded)
                {
                    //vuser.IdentityUserId = new Guid(user.Id);
                    var result = await UserManager.AddToRoleAsync(user.Id, RoleName.Candidate.ToString());
                    if (result.Succeeded)
                    {
                        //var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        //if (SendWelcomeMail(user.Email, user.FirstName, code))
                        //{
                        //    var retVal = "succeeded";
                        //}
                    }

                }
                else
                {
                    ModelState.AddModelError("", userResult.Errors.First());

                }
                return user;
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public async Task<ActionResult> SendEmail(FormCollection email)
        //{
        //    string retVal = "failed";
        //    if (!string.IsNullOrEmpty(email[1]))
        //    {
        //        var userDetails = await UserManager.FindByEmailAsync(email[1]);
        //        var code = await UserManager.GeneratePasswordResetTokenAsync(userDetails.Id);
        //        if (SendWelcomeMail(userDetails.Email, email[1], code))
        //        {
        //            retVal = "succeeded";
        //        }
        //    }

        //    return Json(retVal);
        //}

        //private static string GenerateEncodedKey(string username, string guid)
        //{
        //    byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + "new" + guid);
        //    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    string hashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
        //    return hashParams;
        //}

        //private bool SendWelcomeMail(string email, string userFirstName, object key)
        //{
        //    bool retVal = false;
        //    try
        //    {
        //        string urlEncodedUserName = System.Web.HttpUtility.UrlEncode(email); // url encoded
        //        string subject = ConfigurationManager.AppSettings["ProductNameLong"] + ": " +
        //                         ConfigurationManager.AppSettings["SmtpMailSubjectWelcome"];
        //        string baseUrl = ConfigurationManager.AppSettings["SmtpMailbaseUrl"];

        //        string link = baseUrl + "/CandidateAccount/ResetPassword/?username=" + urlEncodedUserName +
        //                      "&reset=" + GenerateEncodedKey(email, key.ToString());

        //        string body =
        //            "<html>" +
        //            "<body>" +
        //            "<table style='width: 630px; border: none;'><tr><td><table style='border: 1px solid #5C666F; align: left; width: 630px; font-family: arial; font-size: 14px; height: auto;border-spacing: 0;'>" +
        //            "<tr style='width: 630px; height: 44px; border-bottom: 1px solid #5C666F;'>" +
        //            "<td style=' background-color: #00263D; height: 44px; width: 195px; border-bottom: 5px solid #55A51C; margin: 0 auto;'><img src='" +
        //            ConfigurationManager.AppSettings["WebsiteURL"] +
        //            "/Images/rigdig_logo_email.png' style='padding: 3px; margin: 0 auto;' /></td>" +
        //            "<td style='background-color: #5C666F; width: 435px; height: 44px; border-bottom: 5px solid #A3A9AC;vertical-align: middle;'>" +
        //            "<p style='font-size: 19px; margin-left: 20px; color: #fff; font-weight: bold; padding: 0; width: 100%;'>" +
        //            "Welcome to " + ConfigurationManager.AppSettings["ProductNameLong"] + "</p>" +
        //            "</td>" +
        //            "</tr>" +
        //            "<tr>" +
        //            "<td colspan='2' style='width: 630px;width: auto; border: 4px solid #D1D3D4; border-top: none; padding: 30px; margin-top: 4px;'>" +
        //            "<p style='font-size: 14px; color: #000; margin-top: 20px!important;'>" +
        //            "Dear " + userFirstName + "," +
        //            "</p>" +
        //            "<p style='font-size: 14px; color: #000; margin-bottom: 30px;'>" +
        //            "Welcome to " + ConfigurationManager.AppSettings["ProductNameLong"] +
        //            "! Your account contains a wealth of information to help guide your strategic planning, target new prospects, retain customers and better understand your market." +
        //            "</p>" +
        //            "<p style='font-size: 14px; color: #000; margin-bottom: 30px;'>" +
        //            "<span style='color: #55A51C; font-weight: bold;'>" +
        //            "Access " + ConfigurationManager.AppSettings["ProductNameLong"] + "</span>" +
        //            "<br />" +
        //            "You can begin accessing " + ConfigurationManager.AppSettings["ProductNameShort"] +
        //            " with the following login credentials:" +
        //            "</p>" +
        //            "<ul>" +
        //            "<li style='list-style-type:disc; width: 470px; text-decoration: none !important; font-family: arial; font-size: 14px; color: #000; '>" +
        //            "Username: <span style='color: #00698E; text-decoration: none !important;'>" + email +
        //            "</span></li>" +
        //            "<li style='list-style-type:disc; width: 470px;'>" +
        //            "Click to set your Password: " +
        //            "<a href='" + link +
        //            "' target='_blank' style='display: inline; width: 450px; -ms-word-wrap:break-word; word-wrap:break-word; color: #00698E; text-decoration: underline;'>" +
        //            ConfigurationManager.AppSettings["ProductNameShort"] + "_PasswordSetup" +
        //            "</a>" +
        //            "</li>" +
        //            "</ul>" +
        //            "<p style='font-size: 14px; color: #000; margin-top: 30px;'>" +
        //            "<span style='color: #55A51C; font-weight: bold;'>" +
        //            ConfigurationManager.AppSettings["ProductNameShort"] + " Client Success Team" +
        //            "</span>" +
        //            "<br />" +
        //            "We look forward to helping you put the power of " +
        //            ConfigurationManager.AppSettings["ProductNameShort"] +
        //            " Business Intelligence to work for your organization." +
        //            "</p>" +
        //            "<p style='font-size: 14px; color: #000;'>" +
        //            "Please don’t hesitate to contact us with any questions." +
        //            "</p>" +
        //            "<p style='font-size: 14px; color: #000;'>" +
        //            "Kind regards," +
        //            "</p>" +
        //            "<p style='font-size: 14px; color: #000; margin-bottom: 30px; margin-bottom: 10px !important;'>" +
        //            "<span style='font-weight: bold;'>" +
        //            ConfigurationManager.AppSettings["ProductNameLong"] + " Client Success Team" +
        //            "</span>" +
        //            "<br /><span style='color: #000; text-decoration: none!important; font-size: 14px;'>" +
        //            ConfigurationManager.AppSettings["SmtpMailSupportAddress"] + "" +
        //            "</span><br />" +
        //            ConfigurationManager.AppSettings["ContactPhone_DotFormat"] +
        //            "</p>" +
        //            "<p style='text-align:right;margin-bottom: -15px; margin-right: -10px'><img src='" +
        //            ConfigurationManager.AppSettings["WebsiteURL"] +
        //            "/Images/RandallReillyProduct.png' style='padding: 2px;' /></p></td>" +
        //            "</tr></table>" +
        //            "<table style='border: none; width: 630px; border-spacing: 0; align: left; margin: 0; padding: 0;'><tr><td colspan='2' style='border-top: 5px solid #00263D;'>" +
        //            "<p style='color: #000; padding: 10px; margin-top: 10px; font-family: arial; font-size: 12px;'>" +
        //            "NEED HELP?: Training and reference materials are available at " +
        //            "<a href='http://www.silicus.com/' target='_blank' style='color: #00698E; text-decoration: underline;'>" +
        //            "http://www.silicus.com/" +
        //            "</a>" +
        //            "<br />" +
        //            "CONTACT US: We welcome your questions and comments. You can reach us at " +
        //            "<a href='mailto:" + ConfigurationManager.AppSettings["SmtpMailSupportAddress"] +
        //            "' target='_blank' style='color: #00698E; text-decoration: underline;'>" +
        //            ConfigurationManager.AppSettings["SmtpMailSupportAddress"] + "" +
        //            "</a>" +
        //            "</p>" +
        //            "</td></tr></table></td></tr></table>" +
        //            "</body>" +
        //            "</html>";

        //        _emailService.SendEmailAsync(email, subject, body);
        //        retVal = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        retVal = false;
        //        System.Diagnostics.Trace.WriteLine(ex);
        //    }
        //    return retVal;
        //}
    }
}