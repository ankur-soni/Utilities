using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly ICommonDbService _commonDbService;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.Encourage.DAL.Interfaces.IDataContextFactory _dataContextFactory;
        private readonly INominationService _nominationService;
        private readonly IResultService _resultService;
        private readonly ILogger _logger;

        public HomeController(IAwardService awardService, ICommonDbService commonDbService, Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, INominationService nominationService, IResultService resultService, ILogger logger)
        {
            _awardService = awardService;
            _commonDbService = commonDbService;
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _nominationService = nominationService;
            _resultService = resultService;
            _logger = logger;
        }

        public ActionResult Index()
        {
            _logger.Log("Home-Index");
            Dashboard dashboard = GetWinnersList(DateTime.Now.Month, DateTime.Now.Year);
            return View("Dashboard", dashboard);
        }

        public ActionResult GetWinnersListPartialView(int month, int year)
        {
            _logger.Log("Home-GetWinnersListPartialView");
            Dashboard dashboard = GetWinnersList(month, year);
            return PartialView("_winnersList", dashboard);
        }

        private Dashboard GetWinnersList(int month, int year)
        {
            _logger.Log("Home-GetWinnersList");
            int requiredMonth = month != 0 ? month : DateTime.Now.Month;
            int requiredYear = year != 0 ? year : DateTime.Now.Year;

            string utility = WebConfigurationManager.AppSettings["ProductName"];

            var authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());

            var commonRoles = authorizationService.GetRoleForUtility(User.Identity.Name, utility);

            var dashboard = new Dashboard();
            var listOfAwardsAndWinners = new List<DashboardAwardsAndNominations>();

            if ((commonRoles.Count > 0))
            {
                dashboard.userRoles = commonRoles;
                ViewBag.currentUserRoles = commonRoles;
                _logger.Log("No. of roles are:" + commonRoles.Count);
            }
            else
            {
                commonRoles.Add("User");
                dashboard.userRoles = commonRoles;
                ViewBag.currentUserRoles = commonRoles;
                _logger.Log("Current user's role is User");
            }

            var typesOfAwards = _encourageDatabaseContext.Query<Award>().ToList();

            foreach (var award in typesOfAwards)
            {
                var awardsAndNominations = new DashboardAwardsAndNominations();
                awardsAndNominations.AwardId = award.Id;
                awardsAndNominations.AwardTitle = award.Name;

                var winnersForLastMonth = _encourageDatabaseContext.Query<Shortlist>()
                    .Where(w => w.IsWinner == true && w.WinningDate.Value.Month == requiredMonth && w.WinningDate.Value.Year == requiredYear && w.Nomination.AwardId == award.Id)
                    .ToList();

                var listOfWinners = new List<NominationListViewModel>();
                foreach (var winner in winnersForLastMonth)
                {
                    var winnerName = _nominationService.GetNomineeNameOfCurrentNomination(winner.NominationId);
                    var awardMonthYear = _nominationService.GetAwardMonthAndYear(winner.NominationId);
                    var awardName = _nominationService.GetAwardName(winner.NominationId);
                    var awardComment = _resultService.GetAwardComments(winner.NominationId);
                    listOfWinners.Add(new NominationListViewModel() { DisplayName = winnerName, NominationTime = awardMonthYear, AwardName = awardName, AwardComment = awardComment });
                }
                awardsAndNominations.NominationList = listOfWinners;
                listOfAwardsAndWinners.Add(awardsAndNominations);
            }
            dashboard.Awards.AddRange(listOfAwardsAndWinners);

            return dashboard;
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
    }
}