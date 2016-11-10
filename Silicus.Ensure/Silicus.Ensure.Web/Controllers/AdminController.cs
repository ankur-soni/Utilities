﻿using System;
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

namespace Silicus.Ensure.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IQuestionService _questionService;

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

        public AdminController(IEmailService emailService, IQuestionService questionService)
        {
            _emailService = emailService;
            _questionService = questionService;
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

        public ActionResult AddQuestions(int? QuestionId)
        {
            QuestionModel Que = new QuestionModel();
            Que.Success = 0;
            Que.Skills = Skills();
            return View(Que);
        }

        [HttpPost]
        public ActionResult AddQuestions(QuestionModel question)
        {
            Question Que = new Question
            {
                QuestionType = Convert.ToInt32(question.QuestionType),
                QuestionDescription = HttpUtility.HtmlDecode(question.QuestionDescription),
                AnswerType = Convert.ToInt32(question.AnswerType),
                Option1 = question.Option1,
                Option2 = question.Option2,
                Option3 = question.Option3,
                Option4 = question.Option4,
                CorrectAnswer = InlineList(question.CorrectAnswer),
                Answer = HttpUtility.HtmlDecode(question.Answer),
                SkillTag = InlineList(question.SkillTag),
                Competency = Convert.ToInt32(question.Competency),
                Duration = question.Duration,
                IsPublishd = true,
                IsDeleted = false,
                CreatedOn = DateTime.Now,
                CreatedBy = 0,
                ModifiedOn = DateTime.Now,
                ModifiedBy = 0
            };

            int ret = _questionService.Add(Que);

            question = new QuestionModel();
            question.Success = ret;
            question.Skills = Skills();
            return View(question);
        }

        public ActionResult QuestionBank()
        {
            List<QuestionModel> Qmodel = new List<QuestionModel>();
            QuestionModel model;
            IEnumerable<Question> Que = _questionService.GetQuestion();
            foreach (var q in Que)
            {
                model = new QuestionModel();
                model.QuestionId = q.Id;
                model.QuestionDescription = q.QuestionDescription;
                model.QuestionType = GetQuestionType(q.QuestionType);
                model.Skill = GetSkill(q.SkillTag);
                model.Competency = GetCompetency(q.Competency);
                Qmodel.Add(model);
            }
            return View(Qmodel);
        }

        public ActionResult EditQuestion(int QuestionId)
        {
            QuestionModel Que = new QuestionModel();
            Que.Skills = Skills();

            return View("AddQuestions", Que);
        }

        private string InlineList(List<string> list)
        {
            string lst = "";
            if (list != null)
            {
                int cnt = list.Count();
                int commacnt = 0;
                foreach (string str in list)
                {
                    commacnt++;
                    if (commacnt == cnt)
                        lst += str;
                    else
                        lst += str + ",";

                }
            }

            return lst;
        }

        private List<Skills> Skills()
        {
            List<Skills> skill = new List<Skills>();
            skill.Add(new Skills { Skill = "ASP.NET", Value = "1" });
            skill.Add(new Skills { Skill = "MVC 4", Value = "2" });
            skill.Add(new Skills { Skill = "MVC 5", Value = "3" });
            skill.Add(new Skills { Skill = "Java", Value = "4" });
            skill.Add(new Skills { Skill = "CSS", Value = "5" });
            return skill;
        }

        private string GetQuestionType(int type)
        {
            if (type == 1)
                return "Objective";
            else
                return "Practical";
        }

        private string GetCompetency(int type)
        {
            if (type == 1)
                return "Beginner";
            else if (type == 2)
                return "Intermediate";
            else
                return "Expert";
        }

        private string GetSkill(string skill)
        {
            string ret = null;
            if (!string.IsNullOrEmpty(skill))
            {
                string[] str = skill.Split(',');
                List<Skills> skills = Skills();

                foreach (string s in str)
        {
                    ret += skills.Find(x => x.Value == s).Skill;
                    ret += " | ";
                }
            }
            return ret;
        }        



        public ActionResult Candidate()
        {
            return View();
        }      


    }
}
