using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Web.Mappings;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using Silicus.Ensure.Web.Models;
using Silicus.Ensure.Models.Constants;
using System.Data.Entity;

using System.Net.Mail;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Silicus.Ensure.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IQuestionService _questionService;

        private ApplicationUserManager _userManager;
        private readonly ITagsService _tagsService;
        private readonly IMappingService _mappingService;
        private readonly ITestSuiteService _testSuiteService;
        private readonly IUserService _userService;
        private readonly IPositionService _positionService;

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

        public AdminController(IEmailService emailService, ITagsService tagService, ITestSuiteService testSuiteService, MappingService mappingService, IQuestionService questionService, IUserService userService, IPositionService positionService)
        {
            _emailService = emailService;
            _tagsService = tagService;
            _testSuiteService = testSuiteService;
            _mappingService = mappingService;
            _questionService = questionService;
            _userService = userService;
            _positionService = positionService;
        }

        public ActionResult Dashboard()
        {
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> SendEmail(FormCollection email)
        {
            string retVal = "failed";
            if (!string.IsNullOrEmpty(email[1]))
            {
                var userDetails = await UserManager.FindByEmailAsync(email[1]);
                var code = await UserManager.GeneratePasswordResetTokenAsync(userDetails.Id);
                if (SendWelcomeMail(userDetails.Email, email[1], code))
                {
                    retVal = "succeeded";
                }
            }

            return Json(retVal);
        }

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

                _emailService.SendEmailAsync(email, subject, body);
                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
                System.Diagnostics.Trace.WriteLine(ex);
            }
            return retVal;
        }

        private static string GenerateEncodedKey(string username, string guid)
        {
            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + "new" + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string hashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));
            return hashParams;
        }

        #region Candidate

        public ActionResult Candidates()
        {
            ViewBag.UserRoles = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return View();
        }

        public ActionResult CandidatesSuit(int UserId, int IsReassign = 0)
        {
            ViewBag.CurrentUser = UserId;
            ViewBag.IsReassign = IsReassign;
            return PartialView("SelectCandidatesSuit");
        }

        public ActionResult GetPanelDetails([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                bool isAssignedPanel = false;
                var userList = _userService.GetUserDetails();
                var panelList = new List<PanelViewModel>();

                if (userList != null && userList.Any())
                {
                    var panelUserlist = userList.Where(p => p.Role.ToLower() == RoleName.Panel.ToString().ToLower()).ToArray();

                    foreach (var item in panelUserlist)
                    {
                        if (userList.Any(x => x.PanelId != null && x.PanelId.Contains(item.UserId.ToString())))
                        {
                            isAssignedPanel = true;
                        }

                        panelList.Add(new PanelViewModel()
                            {
                                PanelId = item.UserId,
                                PanelName = item.FirstName + " " + item.LastName,
                                IsAssignedPanel = isAssignedPanel
                            });
                    }
                    panelList.OrderBy(x => x.IsAssignedPanel == true);
                }

                DataSourceResult result = panelList.ToDataSourceResult(request);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        public ActionResult AssignPanel(int UserId, int IsReassign = 0)
        {
            ViewBag.CurrentUser = UserId;
            ViewBag.IsReassign = IsReassign;
            return PartialView("_partialSelctPanelList");
        }

        public ActionResult AssignPanelCandidate(string[] PUserId, int UserId, int IsReAssign = 0)
        {
            try
            {
                var panelName = new List<string>(); ;
                foreach (var userId in PUserId)
                {
                    var user = _userService.GetUserById(Convert.ToInt32(userId));
                    if (user != null)
                    {
                        panelName.Add(user.FirstName + " " + user.LastName);
                    }
                }

                var updateUser = _userService.GetUserById(UserId);
                updateUser.PanelId = Convert.ToString(string.Join(",", PUserId));
                updateUser.PanelName = Convert.ToString(string.Join(",", panelName));
                _userService.Update(updateUser);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        public ActionResult AssignSuite(int SuiteId, int UserId, int IsReAssign = 0)
        {
            var updateCurrentUsers = _userService.GetUserDetails().Where(model => model.UserId == UserId).FirstOrDefault();
            if (updateCurrentUsers != null)
            {
                if (SuiteId > 0 && UserId > 0)
                {
                    if (IsReAssign == 1)
                    {
                        var userTest = _testSuiteService.GetUserTestSuite().Where(x => x.UserId == UserId && x.StatusId == Convert.ToInt32(TestStatus.Assigned)).SingleOrDefault();
                        if (userTest != null)
                        {
                            _testSuiteService.DeleteUserTestSuite(userTest);
                        }
                    }
                    var testSuiteDetails = _testSuiteService.GetTestSuiteDetails().Where(model => model.TestSuiteId == SuiteId && model.IsDeleted == false).SingleOrDefault();
                    UserTestSuite userTestSuite = new UserTestSuite();
                    userTestSuite.UserId = UserId;
                    userTestSuite.TestSuiteId = SuiteId;
                    _testSuiteService.AssignSuite(userTestSuite, testSuiteDetails);
                    var selectUser = _userService.GetUserDetails().Where(model => model.UserId == UserId).FirstOrDefault();
                    selectUser.TestStatus = Convert.ToString(TestStatus.Assigned);
                    _userService.Update(selectUser);
                    return Json(1);
                }
                else
                {
                    return Json(-1);
                }
            }
            return View();
        }

        public ActionResult CandidateAdd(int UserId)
        {

            UserViewModel currUser = new UserViewModel();
            currUser.UserId = UserId;

            if (UserId != 0)
            {
                var user = _userService.GetUserById(UserId);
                currUser = _mappingService.Map<User, UserViewModel>(user);
            }
            else if (TempData["UserViewModel"] != null)
            {
                currUser = TempData["UserViewModel"] as UserViewModel;
            }

            var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
            currUser.PositionList = positionDetails.ToList();
            return View(currUser);
        }

        #endregion

        public ActionResult ViewQuestion(int? id)
        {
            int UserId = 0;
            if (id != null)
            {
                UserId = Convert.ToInt32(id);
            }
            var userDetails = _userService.GetUserDetails().Where(x => x.UserId == UserId).FirstOrDefault();
            var userTestSuitDetails = _testSuiteService.GetUserTestSuite().Where(x => x.UserId == UserId).FirstOrDefault().UserTestDetails;

            ViewBag.FNameLName = userDetails.FirstName + userDetails.LastName;

            List<Question> Que = (from question in _questionService.GetQuestion().ToList()
                                  join userTest in userTestSuitDetails.ToList()
                                      on question.Id equals userTest.QuestionId
                                  select question).ToList();

            Que = Que.OrderBy(x => x.Id).ToList();
            ViewBag.UserId = UserId;
            return View(Que);
        }

        public ActionResult CreatePDF()
        {
            List<Question> QList = _questionService.GetQuestion().ToList();
            QList = QList.OrderBy(x => x.Id).ToList();
            var pdf = new RazorPDF.PdfResult(QList, "CreatePDF");
            return pdf;
        }

        public ActionResult SubmittedTest(int canditateId)
        {
            try
            {

                List<ObjectiveQuestionList> objectiveQuestionList = new List<ObjectiveQuestionList>();
                List<PracticalQuestionList> practicalQuestionList = new List<PracticalQuestionList>();

                var userDetails = _userService.GetUserDetails().Where(x => x.UserId == canditateId).FirstOrDefault();

                var userTestSuitDetailsList = _testSuiteService.GetUserTestSuite();
                if (!userTestSuitDetailsList.Any())
                {


                    TempData["ErrorMsg"] = userDetails == null ? "User id can not be null !" : "Test suit is not assigned to user !";
                    return RedirectToAction("Candidates");
                }

                var userTestSuitDetails = userTestSuitDetailsList.Where(x => x.UserId == canditateId).FirstOrDefault();
                var testSuitDetails = _testSuiteService.GetTestSuitById(userTestSuitDetails.TestSuiteId);

                if (testSuitDetails == null)
                {
                    TempData["ErrorMsg"] = "Test suit is not assigned to user !";
                    return RedirectToAction("Candidates");
                }

                SubmittedTestViewModel submittedTestViewModel = new Models.SubmittedTestViewModel();
                submittedTestViewModel.TestStatus = userDetails.TestStatus;
                submittedTestViewModel.FirstName = userDetails.FirstName;
                submittedTestViewModel.LastName = userDetails.LastName;
                submittedTestViewModel.Duration = userTestSuitDetails.Duration;
                submittedTestViewModel.TotalMakrs = userTestSuitDetails.MaxScore;
                submittedTestViewModel.TestSuitName = testSuitDetails.TestSuiteName;
                submittedTestViewModel.UserTestSuiteId = userTestSuitDetails.UserTestSuiteId;
                submittedTestViewModel.Postion = _positionService.GetPositionById(testSuitDetails.Position) != null ? _positionService.GetPositionById(testSuitDetails.Position).PositionName : "";

                foreach (var questionId in userTestSuitDetails.UserTestDetails)
                {

                    var question = _questionService.GetSingleQuestion(questionId.QuestionId);
                    if (question.QuestionType == 1)
                    {
                        if (questionId.Mark != null && questionId.Mark > 0)
                        {
                            submittedTestViewModel.ObjectiveQuestionResult += question.Marks;
                        }
                        submittedTestViewModel.ObjectiveQuestionMarks += question.Marks;

                        objectiveQuestionList.Insert(0, new ObjectiveQuestionList()
                        {
                            QuestionDescription = question.QuestionDescription,
                            CorrectAnswer = GetOption(question.CorrectAnswer),
                            SubmittedAnswer = GetOption(questionId.Answer),
                            Result = questionId.Mark != null && questionId.Mark > 0 ? "Correct" : "Incorrect",
                        });
                    }
                    else
                    {
                        practicalQuestionList.Insert(0, new PracticalQuestionList()
                        {
                            QuestionId = questionId.QuestionId,
                            QuestionDescription = question.QuestionDescription,
                            SubmittedAnswer = questionId.Answer,
                            Weightage = question.Marks,
                            EvaluatedMark = questionId.Mark,
                        });

                    }

                }
                submittedTestViewModel.EvaluatedFeedBack = userTestSuitDetails.FeedBack;
                submittedTestViewModel.TotalMarksObtained = submittedTestViewModel.ObjectiveQuestionResult;
                submittedTestViewModel.objectiveQuestionList = objectiveQuestionList;
                submittedTestViewModel.practicalQuestionList = practicalQuestionList;

                return View(submittedTestViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
                return RedirectToAction("Candidates");
            }
        }

        private string GetOption(string p)
        {
            string optionSelect = "Option:";
            switch (p)
            {
                default: optionSelect += p;
                    break;
            }
            return p == null ? "" : optionSelect;
        }

        [HttpPost]
        public ActionResult SubmittedTest(FormCollection fm)
        {
            int count = 1;

            var userTestSuitDetails = _testSuiteService.GetUserTestSuiteId(Convert.ToInt32(Convert.ToString(Request.Form["UserTestSuiteId"])));

            userTestSuitDetails.EvaluatedMark = Convert.ToInt32(Request.Form["TotalMarksObtained"].ToString());
            userTestSuitDetails.FeedBack = Convert.ToString(Request.Form["EvaluatedFeedBack"]);

            foreach (var userTestDetails in userTestSuitDetails.UserTestDetails.Where(x => x.QuestionId == Convert.ToInt32(Request.Form["PractileQuesionId" + count])).ToList())
            {
                var currentUser = _userService.GetUserByEmail(User.Identity.Name);
                userTestDetails.MarkGivenByName = currentUser != null ? currentUser.FirstName + " " + currentUser.LastName : "";
                userTestDetails.MarkGivenBy = currentUser != null ? currentUser.UserId : 0;
                userTestDetails.Mark = Convert.ToInt32(Request.Form["Emarks" + count]);
                userTestDetails.MarkGivenDate = DateTime.Now;

                _testSuiteService.UpdateUserTestDetails(userTestDetails);
                count++;
            }

            _testSuiteService.UpdateUserTestSuite(userTestSuitDetails);

            return RedirectToAction("Candidates");
        }

        public ActionResult SendMail(int? id)
        {
            int UserId = 0;
            if (id != null)
            {
                UserId = Convert.ToInt32(id);
            }
            var userDetails = _userService.GetUserDetails().Where(x => x.UserId == UserId).FirstOrDefault();
            var userTestSuitDetails = _testSuiteService.GetUserTestSuite().Where(x => x.UserId == UserId).FirstOrDefault().UserTestDetails;

            ViewBag.FNameLName = userDetails.FirstName + userDetails.LastName;

            List<Question> Que = (from question in _questionService.GetQuestion().ToList()
                                  join userTest in userTestSuitDetails.ToList()
                                      on question.Id equals userTest.QuestionId
                                  select question).ToList();

            Que = Que.OrderBy(x => x.Id).ToList();
            //    return View(Que);


            //List<Question> Que = _questionService.GetQuestion().ToList();
            //User user = new User();
            //Que = Que.OrderBy(x => x.Id).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    //user.FirstName = "Nishant";
                    //user.LastName = "Lohakare";
                    var body = "<p>Dear Admin,</p><p>The Online Test has been submitted for <strong>{0} {1}</strong> on " + DateTime.Now + ".</p> Please review, evatuate and add your valuable feedback of the Test in order to conduct first round of interview.<br /><p>Regards,</p><p>Ensure, IT Support</p><p>This is an auto-generated mail sent by Ensure. Please do not reply to this email.</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress("Nishant.Lohakare@silicus.com"));
                    message.From = new MailAddress("nish89.cse@gmail.com", "Ensure Team");
                    message.Subject = "Test Submitted for " + userDetails.FirstName + " " + userDetails.LastName;
                    message.Body = string.Format(body, userDetails.FirstName, userDetails.LastName);

                    string fileName = userDetails.FirstName + "_" + userDetails.FirstName + "_" + userDetails.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    using (System.IO.FileStream fs = new FileStream(Server.MapPath("~\\Attachment") + "\\" + fileName + ".pdf", FileMode.Create))
                    {
                        // Create an instance of the document class which represents the PDF document itself.
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        // Create an instance to the PDF file by creating an instance of the PDF 
                        // Writer class using the document and the filestrem in the constructor.
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();
                        Font Verdana = FontFactory.GetFont("Verdana", 10F, Font.NORMAL, Color.BLACK);
                        document.Add(new Paragraph("Question Set for " + userDetails.FirstName + " " + userDetails.LastName));

                        PdfPTable table1;


                        PdfPTable table2;

                        PdfPCell cell;
                        PdfPCell cell2;

                        document.Add(new Paragraph("Objective Question Set"));
                        foreach (var i in Que)
                        {
                            if (i.QuestionType == 1)
                            {
                                table1 = new PdfPTable(2);
                                table1.SpacingBefore = 20;
                                cell = new PdfPCell(new Phrase(i.QuestionDescription));
                                cell.Rowspan = 4;
                                table1.AddCell(cell);
                                table1.AddCell(i.Option1);
                                table1.AddCell(i.Option2);
                                table1.AddCell(i.Option3);
                                table1.AddCell(i.Option4);
                                cell2 = new PdfPCell(new Phrase("Correct Answer"));
                                table1.AddCell(cell2);
                                table1.AddCell(i.CorrectAnswer);
                                document.Add(table1);

                            }

                        }

                        document.Add(new Paragraph("Practical Question Set"));
                        foreach (var i in Que)
                        {
                            if (i.QuestionType == 2)
                            {
                                table2 = new PdfPTable(2);
                                table2.SpacingBefore = 20;
                                cell = new PdfPCell(new Phrase(i.QuestionDescription));
                                cell.Rowspan = 1;
                                table2.AddCell(cell);
                                table2.AddCell(i.Answer);

                                document.Add(table2);
                            }
                        }
                        // Close the document
                        document.Close();
                        // Close the writer instance
                        writer.Close();
                        // Always close open filehandles explicity
                        fs.Close();

                        Attachment attachment = new Attachment(Server.MapPath("~\\Attachment") + "\\" + fileName + ".pdf");
                        message.Attachments.Add(attachment);
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            smtp.Send(message);
                            TempData["Success"] = "Mail Send Successfully";
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return RedirectToAction("ViewQuestion", "Admin", new { id = UserId });
        }
    }
}
