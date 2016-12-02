using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.Constants;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class PanelController : Controller
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

        public PanelController(IEmailService emailService, ITagsService tagService, ITestSuiteService testSuiteService, MappingService mappingService, IQuestionService questionService, IUserService userService, IPositionService positionService)
        {
            _emailService = emailService;
            _tagsService = tagService;
            _testSuiteService = testSuiteService;
            _mappingService = mappingService;
            _questionService = questionService;
            _userService = userService;
            _positionService = positionService;
        }

        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult PanelAdd(int UserId)
        //{

        //    UserViewModel currUser = new UserViewModel();
        //    currUser.UserId = UserId;

        //    if (UserId != 0)
        //    {
        //        var userList = _userService.GetUserDetails();
        //        var user = userList.FirstOrDefault(x => x.UserId == UserId);
        //        currUser = _mappingService.Map<User, UserViewModel>(user);

        //        if (userList.Any(x => x.PanelId != null && x.PanelId == user.UserId))
        //        {
        //            currUser.CandidateList = new List<int?>();
        //            foreach (var itemUser in userList.Where(x => x.PanelId == user.UserId).ToList())
        //            {

        //                currUser.CandidateList.Add(itemUser.UserId);
        //            }
        //        }

        //    }
        //    else if (TempData["UserViewModel"] != null)
        //    {
        //        currUser = TempData["UserViewModel"] as UserViewModel;
        //    }

        //    var positionDetails = _positionService.GetPositionDetails().OrderBy(model => model.PositionName);
        //    currUser.PositionList = positionDetails.ToList();
        //    ViewBag.candidateList = (from item in _userService.GetUserDetails().Where(x => x.Role.ToLower() == RoleName.Candidate.ToString().ToLower())
        //                             .OrderBy(m => m.FirstName + m.LastName).ToList()
        //                             select new SelectListItem()
        //                             {
        //                                 Text = item.FirstName + " " + item.LastName,
        //                                 Value = item.UserId.ToString()
        //                             }).ToList();
        //    return View(currUser);
        //}
    }
}