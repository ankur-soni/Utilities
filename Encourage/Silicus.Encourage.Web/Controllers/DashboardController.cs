using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Filters;
using Silicus.Encourage.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.Encourage.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly ICommonDbService _commonDbService;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.Encourage.DAL.Interfaces.IDataContextFactory _dataContextFactory;
        private readonly INominationService _nominationService;
        public DashboardController(IAwardService awardService, ICommonDbService commonDbService,Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory,INominationService nominationService)
        {
            _awardService = awardService;
             _commonDbService = commonDbService;
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _nominationService = nominationService;
        }

        [CustomeAuthorize(AllowedRole="User,Manager,Admin,Reviewer")]
        public ActionResult Dashboard()
        {
            var winnersForLastMonth = _encourageDatabaseContext.Query<Shortlist>().Where(w => w.IsWinner == true &&  w.WinningDate.Value.Month == (DateTime.Now.Month) && w.WinningDate.Value.Year == DateTime.Now.Year).ToList();
            var listOfWinners = new List<NominationListViewModel>();
            foreach (var winner in winnersForLastMonth)
            {
                
                var winnerName = _nominationService.GetNomineeNameOfCurrentNomination(winner.NominationId);
                var awardMonthYear = _nominationService.GetAwardMonthAndYear(winner.NominationId);
                var awardName = _nominationService.GetAwardName(winner.NominationId);
               listOfWinners.Add( new NominationListViewModel() { DisplayName = winnerName, NominationTime = awardMonthYear,AwardName = awardName });
                
            }

            return View(listOfWinners);
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