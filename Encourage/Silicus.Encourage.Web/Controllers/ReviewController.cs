using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Models;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Encourage.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IAwardService _awardService;
        private readonly ICommonDbService _commonDbService;
        private readonly ICommonDataBaseContext _commonDbContext;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.Encourage.DAL.Interfaces.IDataContextFactory _dataContextFactory;

        public ReviewController(ICommonDbService commonDbService, Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, IAwardService awardService)
        {
            _commonDbService = commonDbService;
            _commonDbContext = _commonDbService.GetCommonDataBaseContext();
            _dataContextFactory = dataContextFactory;
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _awardService = awardService;
        }
        
        [HttpGet]
        public ActionResult ReviewFeedbackList()
         {
            var reviewFeedbacks = new List<ReviewFeedbackListViewModel>();
            //var allReviewedNominations = _encourageDatabaseContext.Query<Nomination>().Include(model => model.Award).ToList();

            var allReviewedNominations = _encourageDatabaseContext.Query<Review>("Nomination").Where(model => model.IsSubmited == true).ToList();

            foreach(var reviewedNomination in allReviewedNominations)
            {
                var awardCode = _encourageDatabaseContext.Query<Award>().Where(award => award.Id == reviewedNomination.Nomination.AwardId).SingleOrDefault().Code;
                var nominee = _commonDbContext.Query<User>().Where(u => u.ID == reviewedNomination.Nomination.UserId).FirstOrDefault();
                var nominationTime = reviewedNomination.Nomination.NominationDate;
                string nominationTimeToDisplay = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();

                reviewFeedbacks.Add(
                      new ReviewFeedbackListViewModel()
                        {
                            AwardName = awardCode,
                            Credits=0,
                            DisplayName = nominee.DisplayName,
                            Intials = nominee.FirstName.Substring(0, 1) + "" + nominee.LastName.Substring(0, 1),
                            NominationTime = nominationTimeToDisplay,
                            NominationId = reviewedNomination.Nomination.Id
                        }
                     );
            }

          return View();
        }
    }
}