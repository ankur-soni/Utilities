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
        private readonly INominationService _nominationService;
        private readonly IResultService _resultService;
        private readonly ILogger _logger;

        public HomeController(ICommonDbService commonDbService, IDataContextFactory dataContextFactory,
            INominationService nominationService, IResultService resultService, ILogger logger)
        {
            _commonDbService = commonDbService;
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _nominationService = nominationService;
            _resultService = resultService;
            _logger = logger;
        }

        public ActionResult Index()
        {
            _logger.Log("Home-Index");
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.NominationList = GetWinnersList(DateTime.Now.Month, DateTime.Now.Year, 1);

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
            var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId);
            var listOfWinners = new List<NominationListViewModel>();
            if (award != null)
            {
                _logger.Log("Home-GetWinnersList");

                int requiredMonth = month != 0 ? month : DateTime.Now.Month;
                int requiredYear = year != 0 ? year : DateTime.Now.Year;
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
    }
}