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
        private readonly ICommonDbService _commonDbService;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly IDataContextFactory _dataContextFactory;
        private readonly INominationService _nominationService;
        private readonly IResultService _resultService;
        private readonly ILogger _logger;

        public HomeController(ICommonDbService commonDbService, IDataContextFactory dataContextFactory, 
            INominationService nominationService, IResultService resultService, ILogger logger)
        {
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
            DashboardViewModel dashboardViewModel = GetWinnersList(DateTime.Now.Month, DateTime.Now.Year);
            return View("Dashboard", dashboardViewModel);
        }

        public ActionResult GetWinnersListPartialView(int month, int year)
        {
            _logger.Log("Home-GetWinnersListPartialView");
            DashboardViewModel dashboardViewModel = GetWinnersList(month, year);
            return PartialView("_winnersList", dashboardViewModel);
        }

        private DashboardViewModel GetWinnersList(int month, int year)
        {
            _logger.Log("Home-GetWinnersList");
            int requiredMonth = month != 0 ? month : DateTime.Now.Month;
            int requiredYear = year != 0 ? year : DateTime.Now.Year;

            string utiltyName = WebConfigurationManager.AppSettings["ProductName"];

            var authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());

            var commonRoles = authorizationService.GetRoleForUtility(User.Identity.Name, utiltyName);

            var dashboardModel = new DashboardViewModel();
            var listOfAwardsAndWinners = new List<AwardViewModel>();

            if ((commonRoles.Count > 0))
            {
                dashboardModel.userRoles = commonRoles;
                _logger.Log("No. of roles are: " + commonRoles.Count);
            }
            else
            {
                commonRoles.Add("User");
                dashboardModel.userRoles = commonRoles;
                _logger.Log("Current user's role is User");
            }

            var typesOfAwards = _encourageDatabaseContext.Query<Award>().ToList();

            foreach (var award in typesOfAwards)
            {
                var awardsAndNominations = new AwardViewModel
                {
                    AwardId = award.Id,
                    AwardTitle = award.Name,
                    AwardCode = award.Code
                };

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
            dashboardModel.Awards.AddRange(listOfAwardsAndWinners);

            return dashboardModel;
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