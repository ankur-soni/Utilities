using System.Security.Claims;
using System.Web.Security;
using Microsoft.Azure.ActiveDirectory.GraphClient.Internal;
using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using User = Silicus.UtilityContainer.Models.DataObjects.User;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICommonDbService _commonDbService;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly INominationService _nominationService;
        private readonly IResultService _resultService;
        private readonly ILogger _logger;
        private readonly ICustomDateService _customDateService;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDbContext;

        public HomeController(ICommonDbService commonDbService, IDataContextFactory dataContextFactory,
            INominationService nominationService, IResultService resultService, ILogger logger, ICustomDateService customDateService)
        {
            _commonDbService = commonDbService;
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _commonDbContext = commonDbService.GetCommonDataBaseContext();
            _nominationService = nominationService;
            _resultService = resultService;
            _logger = logger;
            _customDateService = customDateService;
        }


        public ActionResult Index()
        {
            _logger.Log("Home-Index");
            var customDate = _customDateService.GetCustomDate(1);
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.NominationList = GetWinnersList(customDate.Month, customDate.Year, 1);

            string utiltyName = WebConfigurationManager.AppSettings["ProductName"];

            var authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());

            var commonRoles = authorizationService.GetRoleForUtility(User.Identity.Name, utiltyName);

            if ((commonRoles.Count > 0))
            {
                dashboardViewModel.UserRoles = commonRoles;
                _logger.Log("No. of roles are: " + commonRoles.Count);
            }
            else
            {
                commonRoles.Add("User");
                dashboardViewModel.UserRoles = commonRoles;
                _logger.Log("Current user's role is User");
            }

            var typesOfAwards = _encourageDatabaseContext.Query<Award>().ToList();
            var awardsList = new List<AwardViewModel>();

            foreach (var award in typesOfAwards)
            {
                var awardDetails = new AwardViewModel
                {
                    AwardId = award.Id,
                    AwardTitle = award.Name,
                    AwardCode = award.Code
                };
                awardsList.Add(awardDetails);
            }
            dashboardViewModel.Awards = awardsList;
            dashboardViewModel.CustomDate = customDate;
            return View("Dashboard", dashboardViewModel);
        }

        public ActionResult GetWinnersListPartialView(int month, int year, int awardId)
        {
            _logger.Log("Home-GetWinnersListPartialView");

            var nominationsList = GetWinnersList(month, year, awardId);
            return PartialView("_winnersList", nominationsList);
        }

        private List<NominationListViewModel> GetWinnersList(int month, int year, int awardId)
        {
            var customdate = _customDateService.GetCustomDate(awardId);
            var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId);
            var listOfWinners = new List<NominationListViewModel>();
            if (award != null)
            {
                _logger.Log("Home-GetWinnersList");

                int requiredMonth = month != 0 ? month : customdate.Month;
                int requiredYear = year != 0 ? year : customdate.Year;
                int requiredAwardId = award.Id;

                List<Shortlist> winners;
                switch (award.Code)
                {
                    case "PINNACLE":
                        winners = _encourageDatabaseContext.Query<Shortlist>()
                        .Where(w =>
                            w.IsWinner == true &&
                            w.Nomination.AwardId == requiredAwardId &&
                            w.WinningDate.Value.Year == requiredYear

                            ).ToList();
                        break;
                    default:
                        winners = _encourageDatabaseContext.Query<Shortlist>()
                       .Where(w =>
                           w.IsWinner == true &&
                           w.WinningDate.Value.Month == requiredMonth &&
                           w.Nomination.AwardId == requiredAwardId &&
                           w.WinningDate.Value.Year == requiredYear
                           ).ToList();
                        break;
                }

                foreach (var winner in winners)
                {
                    var winnerName = _nominationService.GetNomineeNameOfCurrentNomination(winner.NominationId);
                    var awardMonthYear = _nominationService.GetAwardMonthAndYear(winner.NominationId);
                    var awardName = _nominationService.GetAwardName(winner.NominationId);
                    var awardComment = _resultService.GetAwardComments(winner.NominationId);
                    listOfWinners.Add(new NominationListViewModel() { DisplayName = winnerName, NominationTime = awardMonthYear, AwardName = awardName, AwardComment = awardComment });
                }
            }

            return listOfWinners;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult LoginAs()
        {
            var usersWithMultipleRoles = _commonDbService.GetUserWithMultipleRoles();
            //LoginAsViewModel loginAsVM = new LoginAsViewModel();
            //loginAsVM.UsersWithMultipleRoles = new SelectList(usersWithMultipleRoles, "EmailAddress", "DisplayName");
            return View();
        }

        [HttpPost]
        [CustomeAuthorize(AllowedRole = "Admin")]
        public ActionResult LoginAs(LoginAsViewModel loginAs)
        {
            if (!string.IsNullOrEmpty(loginAs.Username))
            {
                var superUserName = User.Identity.Name;

                var user = _commonDbContext.Query<User>().FirstOrDefault(u => u.EmailAddress.ToLower() == loginAs.Username.ToLower());
                if (user == null)
                {
                    ModelState.AddModelError("error", "User does not exists");
                    return View();
                }
                string utility = WebConfigurationManager.AppSettings["ProductName"];
                var roles = _commonDbContext.Query<UtilityUserRoles>().Where(x => x.User.EmailAddress.ToLower() == loginAs.Username.ToLower() && x.Utility.Name.ToLower() == utility.ToLower()).Select(x => x.Role.Name).ToList();

                if (roles.Contains("Manager") || roles.Contains("Reviewer"))
                {
                    User.AddUpdateClaim(ClaimTypes.Name, user.EmailAddress);
                    User.AddUpdateClaim(ClaimTypes.Upn, user.EmailAddress);
                    User.AddUpdateClaim(ClaimTypes.Surname, user.LastName);
                    User.AddUpdateClaim(ClaimTypes.GivenName, user.FirstName);

                    if (string.IsNullOrEmpty(User.GetClaimValue("RootUserName")))
                    {
                        User.AddUpdateClaim("RootUserName", superUserName);
                    }

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("error", "User does not have required permissions");

            }
            else
            {
                ModelState.AddModelError("error", "User name is required");
            }
            return View();
        }
        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}