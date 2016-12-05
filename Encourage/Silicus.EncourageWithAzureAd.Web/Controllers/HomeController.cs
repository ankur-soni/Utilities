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

            string utility = WebConfigurationManager.AppSettings["ProductName"];

            var authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());

            var commonRoles = authorizationService.GetRoleForUtility(User.Identity.Name, utility);
            var dashboard = new Dashboard();
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

            var winnersForLastMonth = _encourageDatabaseContext.Query<Shortlist>().Where(w => w.IsWinner == true && w.WinningDate.Value.Month <= (DateTime.Now.Month) && w.WinningDate.Value.Year == DateTime.Now.Year).ToList();
            var listOfWinners = new List<NominationListViewModel>();
            foreach (var winner in winnersForLastMonth)
            {
                var winnerName = _nominationService.GetNomineeNameOfCurrentNomination(winner.NominationId);
                var awardMonthYear = _nominationService.GetAwardMonthAndYear(winner.NominationId);
                var awardName = _nominationService.GetAwardName(winner.NominationId);
                var awardComment = _resultService.GetAwardComments(winner.NominationId);
                listOfWinners.Add(new NominationListViewModel() { DisplayName = winnerName, NominationTime = awardMonthYear, AwardName = awardName, AwardComment = awardComment });
            }
            dashboard.NominationList = listOfWinners;
            return View("Dashboard", dashboard);
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